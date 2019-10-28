using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hakumuchu
{
    public class HumanoidControllerSample: MonoBehaviour
    {
        [SerializeField]
        private Animator targetAnimator;

        [SerializeField]
        private Hakumuchu.HakumuchuController.Hakumuchu3DoFController ControllerInputDevice;

        [SerializeField]
        private bool MirrorController = true;

        [System.Serializable]
        public struct ApplyConfig
        {
            public Hakumuchu.PoseController.BodyParts BodyParts;
            public HumanBodyBones TargetBonse;
            public Vector3 DefaultRotation;
        }
        [SerializeField]
        public ApplyConfig[] applyConfigs = new ApplyConfig[]
        {
            new ApplyConfig() {
                BodyParts = PoseController.BodyParts.Shoulder,
                TargetBonse = HumanBodyBones.LeftUpperArm,
                DefaultRotation = new Vector3(15.6f, 90.2f, 90.1f) },
            new ApplyConfig() {
                BodyParts = PoseController.BodyParts.Elbow,
                TargetBonse = HumanBodyBones.LeftLowerArm,
                DefaultRotation = new Vector3(15.4f, 88.0f, 359.5f) },
            new ApplyConfig() {
                BodyParts = PoseController.BodyParts.Wrist,
                TargetBonse = HumanBodyBones.LeftHand,
                DefaultRotation = new Vector3(52.0f, 82.0f, 354.0f) },
        };



        [SerializeField]
        private Hakumuchu.PoseController.SwingArmEstimator armEstimator;

        private Dictionary<HumanBodyBones, Quaternion> poseBackup = new Dictionary<HumanBodyBones, Quaternion>();

        void Awake()
        {
            foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
            {
                if (HumanBodyBones.LastBone <= bone || this.targetAnimator.GetBoneTransform(bone) == null) continue;
                poseBackup.Add(bone, this.targetAnimator.GetBoneTransform(bone).rotation);
            }
            Debug.Log("rot: " + poseBackup[HumanBodyBones.LeftUpperArm].eulerAngles);
            Debug.Log("rot: " + poseBackup[HumanBodyBones.LeftLowerArm].eulerAngles);
            Debug.Log("rot: " + poseBackup[HumanBodyBones.LeftHand].eulerAngles);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void LateUpdate()
        {
            this.targetAnimator.GetBoneTransform(HumanBodyBones.Spine).rotation
                = this.transform.rotation * poseBackup[HumanBodyBones.Spine];


            if (!ControllerInputDevice.IsConnected) return;

            Hakumuchu.PoseController.ArmEstimator.Input armIn = new Hakumuchu.PoseController.ArmEstimator.Input()
            {
                IsRightHand = false,
                NeckPosition = Vector3.zero,
                TorsoRotation = this.transform.rotation,
                ControllerRotation = MirrorController? ControllerInputDevice.MirrorOrientation: ControllerInputDevice.Orientation
            };
            Hakumuchu.PoseController.ArmEstimator.Output armOut = armEstimator.Estimate(armIn);

            Dictionary<PoseController.BodyParts, Pose> poses = armOut.Poses;

            foreach(ApplyConfig config in applyConfigs)
            {
                Quaternion rot = poses[config.BodyParts].rotation;
                Quaternion rst = Quaternion.Euler(config.DefaultRotation);
                targetAnimator.GetBoneTransform(config.TargetBonse).rotation = rot * rst;
            }
        }


    }
}

