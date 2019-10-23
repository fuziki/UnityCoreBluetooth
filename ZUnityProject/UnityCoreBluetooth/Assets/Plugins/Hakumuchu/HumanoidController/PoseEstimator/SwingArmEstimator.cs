using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwingArmEstimator: ArmEstimator
{
    [SerializeField]
    private AnimationCurve shoulderRotationRatioCurve = new AnimationCurve(new Keyframe[]
    {
        //Keyframe(float time, float value, float inTangent, float outTangent, float inWeight, float outWeight)
        new Keyframe(90f, 0.6f, 0f, 0f, 0.001f, 0.001f),
        new Keyframe(0.0f, 0.4f, 0f, 0f, 0.001f, 0.001f),
        new Keyframe(-90f, 0.0f, 0f, 0f, 0.001f, 0.001f),
    });

    [SerializeField]
    private AnimationCurve elbowRotationRatioCurve = new AnimationCurve(new Keyframe[]
    {
        //Keyframe(float time, float value, float inTangent, float outTangent, float inWeight, float outWeight)
        new Keyframe(90f, 0.3f, 0f, 0f, 0.001f, 0.001f),
        new Keyframe(0.0f, 0.3f, 0f, 0f, 0.001f, 0.001f),
        new Keyframe(-90f, 1.0f, 0f, 0f, 0.001f, 0.001f),
    });

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


    protected override void CalculateFinalJointRotations(Input input,
                                                        ref Output output,
                                                        ControllerRotation controllerRotation,
                                                        Quaternion lerpRotation)
    {

        // As the controller angle increases the ratio of the rotation applied to each joint shifts.
        float totalAngle = Quaternion.Angle(controllerRotation.XYRotation, Quaternion.identity);
        float joingShiftAngleRange = maxJointShiftAngle - minJointShiftAngle;
        float angleRatio = Mathf.Clamp01((totalAngle - minJointShiftAngle) / joingShiftAngleRange);
        float jointShiftRatio = Mathf.Pow(angleRatio, jointShiftExponent);

        // Calculate what portion of the rotation is applied to each joint.
        float finalShoulderRatio = Mathf.Lerp(shoulderRotationRatioCurve.Evaluate(controllerRotation.XAngle), shiftedShoulderRotationRatio, jointShiftRatio);
        float finalElbowRatio = Mathf.Lerp(elbowRotationRatioCurve.Evaluate(controllerRotation.XAngle), shiftedElbowRotationRatio, jointShiftRatio);
        float finalWristRatio = Mathf.Lerp(wristRotationRatio, shiftedWristRotationRatio, jointShiftRatio);

        // Calculate relative rotations for each joint.
        Quaternion swingShoulderRot = Quaternion.Lerp(Quaternion.identity, controllerRotation.XYRotation, finalShoulderRatio);
        Quaternion swingElbowRot = Quaternion.Lerp(Quaternion.identity, controllerRotation.XYRotation, finalElbowRatio);
        Quaternion swingWristRot = Quaternion.Lerp(Quaternion.identity, controllerRotation.XYRotation, finalWristRatio);

        // Calculate final rotations.
        output.ShoulderRotation = input.TorsoRotation * swingShoulderRot;
        output.ElbowRotation = output.ShoulderRotation * swingElbowRot;
        output.WristRotation = output.ElbowRotation * swingWristRot;
        output.ControllerRotation = input.TorsoRotation * controllerRotation.Orientation;
//        Debug.Log("xy: " + xyRotation.eulerAngles + ", state: " + state.torsoRotation.eulerAngles + state.shoulderRotation.eulerAngles + state.elbowRotation.eulerAngles + state.wristRotation.eulerAngles);
    }
}
