using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hakumuchu.ArmModel
{
    public enum BodyParts
    {
        Head, Neck, Torso, Shoulder, Elbow, Wrist
    }
}

namespace Hakumuchu
{
    public class HumanoidController: MonoBehaviour
    {
        [SerializeField]
        private Animator targetAnimator;

        [SerializeField]
        private Hakumuchu.HakumuchuController.Hakumuchu3DoFController ControllerInputDevice;

        [SerializeField]
        private bool MirrorController = true;

        [SerializeField]
        private PartsBonePair[] partsToBone = new PartsBonePair[]
        {
            new PartsBonePair(){ key = ArmModel.BodyParts.Torso, value = HumanBodyBones.Spine },
            new PartsBonePair(){ key = ArmModel.BodyParts.Shoulder, value = HumanBodyBones.RightUpperArm },
            new PartsBonePair(){ key = ArmModel.BodyParts.Elbow, value = HumanBodyBones.RightLowerArm },
            new PartsBonePair(){ key = ArmModel.BodyParts.Wrist, value = HumanBodyBones.RightHand },
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
            Debug.Log("rot: " + poseBackup[HumanBodyBones.LeftLowerArm].eulerAngles);
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
            Hakumuchu.PoseController.ArmEstimator.Input armIn = new Hakumuchu.PoseController.ArmEstimator.Input()
            {
                IsRightHand = false,
                NeckPosition = Vector3.zero,
                TorsoRotation = this.transform.rotation,
                ControllerRotation = MirrorController? ControllerInputDevice.MirrorOrientation: ControllerInputDevice.Orientation
            };
            Hakumuchu.PoseController.ArmEstimator.Output armOut = armEstimator.Estimate(armIn);

            foreach (PartsBonePair pair in this.partsToBone)
            {
                Quaternion rot;
                switch (pair.key)
                {
                    case ArmModel.BodyParts.Torso:
                        rot = this.transform.rotation;
                        break;
                    case ArmModel.BodyParts.Shoulder:
                        rot = armOut.ShoulderRotation;
                        break;
                    case ArmModel.BodyParts.Elbow:
                        rot = armOut.ElbowRotation;
                        break;
                    case ArmModel.BodyParts.Wrist:
                        rot = armOut.WristRotation;
                        break;
                    default: continue;
                }
                this.targetAnimator.GetBoneTransform(pair.value).rotation = rot * poseBackup[pair.value];
            }
        }

    }
}

