using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HmcSwingArmModel : HmcArmModel
{
    [Tooltip("Portion of controller rotation applied to the shoulder joint.")]
    [Range(0.0f, 1.0f)]
    public float shoulderRotationRatio = 0.5f;

    [Tooltip("Portion of controller rotation applied to the elbow joint.")]
    [Range(0.0f, 1.0f)]
    public float elbowRotationRatio = 0.3f;

    [Tooltip("Portion of controller rotation applied to the wrist joint.")]
    [Range(0.0f, 1.0f)]
    public float wristRotationRatio = 0.2f;

    [Tooltip("Min angle of the controller before starting to lerp towards the shifted joint ratios.")]
    [Range(0.0f, 180.0f)]
    public float minJointShiftAngle = 160.0f;

    [Tooltip("Max angle of the controller before the lerp towards the shifted joint ratios ends.")]
    [Range(0.0f, 180.0f)]
    public float maxJointShiftAngle = 180.0f;

    [Tooltip("Exponent applied to the joint shift ratio to control the curve of the shift.")]
    [Range(1.0f, 20.0f)]
    public float jointShiftExponent = 6.0f;

    [Tooltip("Portion of controller rotation applied to the shoulder joint when the controller is backwards.")]
    [Range(0.0f, 1.0f)]
    public float shiftedShoulderRotationRatio = 0.1f;

    [Tooltip("Portion of controller rotation applied to the elbow joint when the controller is backwards.")]
    [Range(0.0f, 1.0f)]
    public float shiftedElbowRotationRatio = 0.4f;

    [Tooltip("Portion of controller rotation applied to the wrist joint when the controller is backwards.")]
    [Range(0.0f, 1.0f)]
    public float shiftedWristRotationRatio = 0.5f;

    protected override void CalculateFinalJointRotations(Quaternion controllerOrientation, Quaternion xyRotation, Quaternion lerpRotation)
    {
        // As the controller angle increases the ratio of the rotation applied to each joint shifts.
        float totalAngle = Quaternion.Angle(xyRotation, Quaternion.identity);
        float joingShiftAngleRange = maxJointShiftAngle - minJointShiftAngle;
        float angleRatio = Mathf.Clamp01((totalAngle - minJointShiftAngle) / joingShiftAngleRange);
        float jointShiftRatio = Mathf.Pow(angleRatio, jointShiftExponent);

        // Calculate what portion of the rotation is applied to each joint.
        float finalShoulderRatio = Mathf.Lerp(shoulderRotationRatio, shiftedShoulderRotationRatio, jointShiftRatio);
        float finalElbowRatio = Mathf.Lerp(elbowRotationRatio, shiftedElbowRotationRatio, jointShiftRatio);
        float finalWristRatio = Mathf.Lerp(wristRotationRatio, shiftedWristRotationRatio, jointShiftRatio);

        // Calculate relative rotations for each joint.
        Quaternion swingShoulderRot = Quaternion.Lerp(Quaternion.identity, xyRotation, finalShoulderRatio);
        Quaternion swingElbowRot = Quaternion.Lerp(Quaternion.identity, xyRotation, finalElbowRatio);
        Quaternion swingWristRot = Quaternion.Lerp(Quaternion.identity, xyRotation, finalWristRatio);

        // Calculate final rotations.
        Quaternion shoulderRotation = state.torsoRotation * swingShoulderRot;
        state.elbowRotation = shoulderRotation * swingElbowRot;
        state.wristRotation = state.elbowRotation * swingWristRot;
        state.controllerRotation = state.torsoRotation * controllerOrientation;
        state.torsoRotation = shoulderRotation;
    }
}
