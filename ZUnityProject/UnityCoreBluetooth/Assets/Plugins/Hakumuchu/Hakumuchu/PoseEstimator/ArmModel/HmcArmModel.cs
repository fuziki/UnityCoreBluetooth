using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHMCArmModel
{
    Quaternion TorsoRotation { get; }
    Quaternion ShoulderRotation { get; }
    Quaternion ElbowRotation { get; }
    Quaternion WristRotation { get; }
}

public class HmcArmModel : MonoBehaviour, IHMCArmModel {


    [SerializeField]
    private Transform TorsoTransdorm;

    protected static readonly Vector3 NECK_OFFSET = new Vector3(0.0f, 0.075f, 0.08f);
    public static readonly Vector3 DEFAULT_ELBOW_REST_POSITION = new Vector3(0.195f, -0.5f, 0.005f);
    public static readonly Vector3 DEFAULT_WRIST_REST_POSITION = new Vector3(0.0f, 0.0f, 0.25f);
    public static readonly Vector3 DEFAULT_CONTROLLER_REST_POSITION = new Vector3(0.0f, 0.0f, 0.05f);
    protected const float MIN_EXTENSION_ANGLE = 7.0f;
    protected const float MAX_EXTENSION_ANGLE = 60.0f;
    public static readonly Vector3 DEFAULT_ARM_EXTENSION_OFFSET = new Vector3(-0.13f, 0.14f, 0.08f);
    public const float DEFAULT_ELBOW_BEND_RATIO = 0.6f;
    protected const float EXTENSION_WEIGHT = 0.4f;


    public HakumuchuController ControllerInputDevice;

    protected virtual void OnEnable()
    {
        // Register the controller update listener.
        //        GvrControllerInput.OnControllerInputUpdated += OnControllerInputUpdated;

        // Force the torso direction to match the gaze direction immediately.
        // Otherwise, the controller will not be positioned correctly if the ArmModel was enabled
        // when the user wasn't facing forward.
        UpdateTorsoDirection(true);

        // Update immediately to avoid a frame delay before the arm model is applied.
        OnControllerInputUpdated();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.OnControllerInputUpdated();

    }

    [System.Serializable]
    public class Config
    {
        public bool isLockedToNeck = true;
        public Vector3 elbowRestPosition = DEFAULT_ELBOW_REST_POSITION;
        public Vector3 wristRestPosition = DEFAULT_WRIST_REST_POSITION;
        public Vector3 controllerRestPosition = DEFAULT_CONTROLLER_REST_POSITION;
        public Vector3 armExtensionOffset = DEFAULT_ARM_EXTENSION_OFFSET;
        [Range(0.0f, 1.0f)]
        public float elbowBendRatio = DEFAULT_ELBOW_BEND_RATIO;
    }
    [SerializeField]
    protected Config config = new Config();

    public class State
    {
        public Vector3 handedMultiplier = new Vector3();
        public Vector3 torsoDirection = Vector3.forward;
        public Quaternion torsoRotation = new Quaternion();
        public Quaternion shoulderRotation = new Quaternion();
        public Vector3 neckPosition = new Vector3();
        public Vector3 elbowPosition = new Vector3();
        public Vector3 wristPosition = new Vector3();
        public Vector3 controllerPosition = new Vector3();
        public Quaternion elbowRotation = new Quaternion();
        public Quaternion wristRotation = new Quaternion();
        public Quaternion controllerRotation = new Quaternion();
    }
    protected State state = new State();

    public Quaternion TorsoRotation => this.state.torsoRotation;
    public Quaternion ShoulderRotation => this.state.shoulderRotation;
    public Quaternion ElbowRotation => this.state.elbowRotation;
    public Quaternion WristRotation => this.state.wristRotation;



    protected virtual void OnControllerInputUpdated()
    {
        UpdateHandedness();
        UpdateTorsoDirection(false);
        UpdateNeckPosition();
        ApplyArmModel();
    }

    protected virtual void UpdateHandedness()
    {
        if (ControllerInputDevice.IsRightHand)
            this.state.handedMultiplier.Set(1, 1, 1);
        else
            this.state.handedMultiplier.Set(-1, 1, 1);
    }

    protected virtual void UpdateTorsoDirection(bool forceImmediate)
    {
        // Determine the gaze direction horizontally.
        Vector3 gazeDirection = GvrVRHelpers.GetHeadForward();
        gazeDirection.y = 0.0f;
        gazeDirection.Normalize();

        // Use the gaze direction to update the forward direction.
        if (forceImmediate ||
              (ControllerInputDevice != null && ControllerInputDevice.Recentered))
        {
            this.state.torsoDirection = gazeDirection;
        }
        else
        {
            float angularVelocity =
                ControllerInputDevice != null ? ControllerInputDevice.Gyro.magnitude : 0;

            float gazeFilterStrength = Mathf.Clamp((angularVelocity - 0.2f) / 45.0f, 0.0f, 0.1f);
            this.state.torsoDirection = Vector3.Slerp(this.state.torsoDirection, gazeDirection, gazeFilterStrength);
        }

        // Calculate the torso rotation.
//        this.state.torsoRotation = Quaternion.FromToRotation(Vector3.forward, this.state.torsoDirection);
        this.state.torsoRotation = this.TorsoTransdorm.rotation;
    }


    protected virtual void UpdateNeckPosition()
    {
        if (config.isLockedToNeck)
        {
            // Returns the center of the eyes.
            // However, we actually want to lock to the center of the head.
            this.state.neckPosition = GvrVRHelpers.GetHeadPosition();

            // Find the approximate neck position by Applying an inverse neck model.
            // This transforms the head position to the center of the head and also accounts
            // for the head's rotation so that the motion feels more natural.
            this.state.neckPosition = ApplyInverseNeckModel(this.state.neckPosition);
        }
        else
        {
            this.state.neckPosition = Vector3.zero;
        }
    }

    protected virtual Vector3 ApplyInverseNeckModel(Vector3 headPosition)
    {
        Quaternion headRotation = GvrVRHelpers.GetHeadRotation();
        Vector3 rotatedNeckOffset =
            (headRotation * NECK_OFFSET) - (NECK_OFFSET.y * Vector3.up);
        headPosition -= rotatedNeckOffset;

        return headPosition;
    }

    protected virtual void ApplyArmModel()
    {
        // Set the starting positions of the joints before they are transformed by the arm model.
        SetUntransformedJointPositions();

        // Get the controller's orientation.
        Quaternion controllerOrientation;
        Quaternion xyRotation;
        float xAngle;
        GetControllerRotation(out controllerOrientation, out xyRotation, out xAngle);

        // Offset the elbow by the extension offset.
        float extensionRatio = CalculateExtensionRatio(xAngle);
        ApplyExtensionOffset(extensionRatio);

        // Calculate the lerp rotation, which is used to control how much the rotation of the
        // controller impacts each joint.
        Quaternion lerpRotation = CalculateLerpRotation(xyRotation, extensionRatio);

        CalculateFinalJointRotations(controllerOrientation, xyRotation, lerpRotation);
        ApplyRotationToJoints();
    }

    protected virtual void SetUntransformedJointPositions()
    {
        this.state.elbowPosition = Vector3.Scale(this.config.elbowRestPosition, this.state.handedMultiplier);
        this.state.wristPosition = Vector3.Scale(this.config.wristRestPosition, state.handedMultiplier);
        this.state.controllerPosition = Vector3.Scale(this.config.controllerRestPosition, state.handedMultiplier);
    }

    protected void GetControllerRotation(out Quaternion rotation,
                                         out Quaternion xyRotation,
                                         out float xAngle)
    {
        // Find the controller's orientation relative to the player.
        rotation = ControllerInputDevice != null ?
            ControllerInputDevice.Orientation : Quaternion.Euler(0, 0, 0);

        rotation = Quaternion.Inverse(this.state.torsoRotation) * rotation;

        Debug.Log("torsoRotation: " + state.torsoRotation.eulerAngles + ", rotation: " + rotation.eulerAngles);

        // Extract just the x rotation angle.
        Vector3 controllerForward = rotation * Vector3.forward;
        xAngle = 90.0f - Vector3.Angle(controllerForward, Vector3.up);

        // Remove the z rotation from the controller.
        xyRotation = Quaternion.FromToRotation(Vector3.forward, controllerForward);
    }

    protected virtual float CalculateExtensionRatio(float xAngle)
    {
        float normalizedAngle =
            (xAngle - MIN_EXTENSION_ANGLE) / (MAX_EXTENSION_ANGLE - MIN_EXTENSION_ANGLE);

        float extensionRatio = Mathf.Clamp(normalizedAngle, 0.0f, 1.0f);
        return extensionRatio;
    }

    protected virtual void ApplyExtensionOffset(float extensionRatio)
    {
        Vector3 extensionOffset = Vector3.Scale(this.config.armExtensionOffset, this.state.handedMultiplier);
        this.state.elbowPosition += extensionOffset * extensionRatio;
    }

    protected virtual Quaternion CalculateLerpRotation(Quaternion xyRotation, float extensionRatio)
    {
        float totalAngle = Quaternion.Angle(xyRotation, Quaternion.identity);
        float lerpSuppresion = 1.0f - Mathf.Pow(totalAngle / 180.0f, 6.0f);
        float inverseElbowBendRatio = 1.0f - this.config.elbowBendRatio;

        float lerpValue =
            inverseElbowBendRatio + (this.config.elbowBendRatio * extensionRatio * EXTENSION_WEIGHT);

        lerpValue *= lerpSuppresion;
        return Quaternion.Lerp(Quaternion.identity, xyRotation, lerpValue);
    }

    protected virtual void CalculateFinalJointRotations(Quaternion controllerOrientation,
                                                    Quaternion xyRotation,
                                                    Quaternion lerpRotation)
    {
        this.state.elbowRotation = this.state.torsoRotation * Quaternion.Inverse(lerpRotation) * xyRotation;
        this.state.wristRotation = this.state.elbowRotation * lerpRotation;
        this.state.controllerRotation = this.state.torsoRotation * controllerOrientation;
    }


    protected virtual void ApplyRotationToJoints()
    {
        this.state.elbowPosition = this.state.neckPosition + (this.state.torsoRotation * this.state.elbowPosition);
        this.state.wristPosition = this.state.elbowPosition + (this.state.elbowRotation * this.state.wristPosition);
        this.state.controllerPosition = this.state.wristPosition + (this.state.wristRotation * this.state.controllerPosition);
    }


}
