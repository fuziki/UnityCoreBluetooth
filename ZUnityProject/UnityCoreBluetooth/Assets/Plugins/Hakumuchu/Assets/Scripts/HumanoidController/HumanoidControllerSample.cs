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

        [SerializeField]
        private PartsBonePair[] partsToBone = new PartsBonePair[]
        {
            new PartsBonePair(){ key = Hakumuchu.PoseController.BodyParts.Torso, value = HumanBodyBones.Spine },
            new PartsBonePair(){ key = Hakumuchu.PoseController.BodyParts.Shoulder, value = HumanBodyBones.RightUpperArm },
            new PartsBonePair(){ key = Hakumuchu.PoseController.BodyParts.Elbow, value = HumanBodyBones.RightLowerArm },
            new PartsBonePair(){ key = Hakumuchu.PoseController.BodyParts.Wrist, value = HumanBodyBones.RightHand },
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
            float lsh = Input.GetAxis("L_Stick_H");
            float lsv = Input.GetAxis("L_Stick_V");
            if ((lsh != 0) || (lsv != 0))
            {
                Debug.Log("L stick:" + lsh + "," + lsv);
            }
            //R Stick
            float rsh = Input.GetAxis("R_Stick_H");
            float rsv = Input.GetAxis("R_Stick_V");
            if ((rsh != 0) || (rsv != 0))
            {
                Debug.Log("R stick:" + rsh + "," + rsv);
            }
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
                    case Hakumuchu.PoseController.BodyParts.Torso:
                        rot = this.transform.rotation;
                        break;
                    case Hakumuchu.PoseController.BodyParts.Shoulder:
                        rot = armOut.ShoulderRotation;
                        break;
                    case Hakumuchu.PoseController.BodyParts.Elbow:
                        rot = armOut.ElbowRotation;
                        break;
                    case Hakumuchu.PoseController.BodyParts.Wrist:
                        rot = armOut.WristRotation;
                        break;
                    default: continue;
                }
                this.targetAnimator.GetBoneTransform(pair.value).rotation = rot * poseBackup[pair.value];
            }
        }

    }
}

