using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArmEstimator {
    public static readonly Vector3 DEFAULT_ELBOW_REST_POSITION = new Vector3(0.195f, -0.5f, 0.005f);
    public static readonly Vector3 DEFAULT_WRIST_REST_POSITION = new Vector3(0.0f, 0.0f, 0.25f);
    public static readonly Vector3 DEFAULT_CONTROLLER_REST_POSITION = new Vector3(0.0f, 0.0f, 0.05f);
    protected const float MIN_EXTENSION_ANGLE = 7.0f;
    protected const float MAX_EXTENSION_ANGLE = 60.0f;
    public static readonly Vector3 DEFAULT_ARM_EXTENSION_OFFSET = new Vector3(-0.13f, 0.14f, 0.08f);
    public const float DEFAULT_ELBOW_BEND_RATIO = 0.6f;
    protected const float EXTENSION_WEIGHT = 0.4f;

    [SerializeField]
    private Vector3 elbowRestPosition = DEFAULT_ELBOW_REST_POSITION;
    [SerializeField]
    private Vector3 wristRestPosition = DEFAULT_WRIST_REST_POSITION;
    [SerializeField]
    private Vector3 controllerRestPosition = DEFAULT_CONTROLLER_REST_POSITION;
    [SerializeField]
    private Vector3 armExtensionOffset = DEFAULT_ARM_EXTENSION_OFFSET;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float elbowBendRatio = DEFAULT_ELBOW_BEND_RATIO;

    public struct Input
    {
        public bool IsRightHand;
        public Vector3 NeckPosition;
        public Quaternion TorsoRotation;
        public Quaternion ControllerRotation;
    }

    public struct Output
    {
        public Quaternion ShoulderRotation;
        public Quaternion ElbowRotation;
        public Quaternion WristRotation;
        public Quaternion ControllerRotation;
        public Vector3 ElbowPosition;
        public Vector3 WristPosition;
        public Vector3 ControllerPosition;
    }

    public struct ControllerRotation
    {
        public Quaternion Orientation;
        public Quaternion XYRotation;
        public float XAngle;
        public ControllerRotation(Quaternion controllerRotation, Quaternion torsoRotation)
        {
            this.Orientation = Quaternion.Inverse(torsoRotation) * controllerRotation;
            this.Orientation = controllerRotation;
            Vector3 controllerForward = this.Orientation * Vector3.forward;
            this.XAngle = XAngle = 90.0f - Vector3.Angle(controllerForward, Vector3.up);
            this.XYRotation = Quaternion.FromToRotation(Vector3.forward, controllerForward);
        }
    }

    private Vector3 handedMultiplier = new Vector3();
    public Output Estimate(Input input)
    {
        ControllerRotation controllerRotation = new ControllerRotation(input.ControllerRotation, input.TorsoRotation);

        if (input.IsRightHand)
            this.handedMultiplier.Set(1, 1, 1);
        else
            this.handedMultiplier.Set(-1, 1, 1);


        Output output = new Output();

        // Set the starting positions of the joints before they are transformed by the arm model.
        SetUntransformedJointPositions(ref output);

        // Offset the elbow by the extension offset.
        float extensionRatio = CalculateExtensionRatio(controllerRotation.XAngle);
        Vector3 offset = ApplyExtensionOffset(extensionRatio);
        output.ElbowPosition += offset;

        // Calculate the lerp rotation, which is used to control how much the rotation of the
        // controller impacts each joint.
        Quaternion lerpRotation = CalculateLerpRotation(controllerRotation.XYRotation, extensionRatio);

        CalculateFinalJointRotations(input, ref output, controllerRotation, lerpRotation);
        ApplyRotationToJoints(input, ref output);

        return output;
    }

    protected virtual void SetUntransformedJointPositions(ref Output output)
    {
        output.ElbowPosition = Vector3.Scale(this.elbowRestPosition, this.handedMultiplier);
        output.WristPosition = Vector3.Scale(this.wristRestPosition, this.handedMultiplier);
        output.ControllerPosition = Vector3.Scale(this.controllerRestPosition, this.handedMultiplier);
    }

    protected virtual float CalculateExtensionRatio(float xAngle)
    {
        float normalizedAngle =
            (xAngle - MIN_EXTENSION_ANGLE) / (MAX_EXTENSION_ANGLE - MIN_EXTENSION_ANGLE);

        float extensionRatio = Mathf.Clamp(normalizedAngle, 0.0f, 1.0f);
        return extensionRatio;
    }

    protected virtual Vector3 ApplyExtensionOffset(float extensionRatio)
    {
        Vector3 extensionOffset = Vector3.Scale(this.armExtensionOffset, this.handedMultiplier);
        return extensionOffset * extensionRatio;
    }

    protected virtual Quaternion CalculateLerpRotation(Quaternion xyRotation, float extensionRatio)
    {
        float totalAngle = Quaternion.Angle(xyRotation, Quaternion.identity);
        float lerpSuppresion = 1.0f - Mathf.Pow(totalAngle / 180.0f, 6.0f);
        float inverseElbowBendRatio = 1.0f - this.elbowBendRatio;

        float lerpValue =
            inverseElbowBendRatio + (this.elbowBendRatio * extensionRatio * EXTENSION_WEIGHT);

        lerpValue *= lerpSuppresion;
        return Quaternion.Lerp(Quaternion.identity, xyRotation, lerpValue);
    }

    protected virtual void CalculateFinalJointRotations(Input input,
                                                        ref Output output, 
                                                        ControllerRotation controllerRotation,
                                                        Quaternion lerpRotation)
    {
        output.ElbowRotation = input.TorsoRotation * Quaternion.Inverse(lerpRotation) * controllerRotation.XYRotation;
        output.WristRotation = output.ElbowRotation * lerpRotation;
        output.ControllerRotation = input.TorsoRotation * controllerRotation.Orientation;
    }

    protected virtual void ApplyRotationToJoints(Input input, ref Output output)
    {
        output.ElbowPosition = input.NeckPosition + (input.TorsoRotation * output.ElbowPosition);
        output.WristPosition = output.ElbowPosition + (output.ElbowRotation * output.WristPosition);
        output.ControllerPosition = output.WristPosition + (output.WristRotation * output.ControllerPosition);
    }
}
