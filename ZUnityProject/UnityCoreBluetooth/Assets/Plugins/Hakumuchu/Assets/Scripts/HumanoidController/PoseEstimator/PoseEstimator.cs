using UnityEngine;

namespace Hakumuchu.PoseController
{
    public enum BodyParts
    {
        Torso, Shoulder, Elbow, Wrist
    }
    public class PoseEstimator : MonoBehaviour
    {
        [SerializeField]
        private TorsoEstimator torsoEstimator;

        [SerializeField]
        private SwingArmEstimator armEstimator;

        // Use this for initialization
        void Start()
        {
            this.torsoEstimator = new TorsoEstimator();
            //        this.armEstimator = new SwingArmEstimator(this.armConfig);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Estimate(Quaternion controllerRotation)
        {
            TorsoEstimator.Input torsoIn = new TorsoEstimator.Input()
            {
                IsLockedToNeck = true,
                HeadPosition = Vector3.zero,
                HeadRotation = Quaternion.identity,
                AngularVelocity = 0.0f,
                ForceImmediate = false
            };

            TorsoEstimator.Output torsoOut = torsoEstimator.Estimate(torsoIn);

            ArmEstimator.Input armIn = new ArmEstimator.Input()
            {
                IsRightHand = true,
                NeckPosition = torsoOut.NeckPosition,
                TorsoRotation = torsoOut.TorsoRotation,
                ControllerRotation = Quaternion.identity
            };
            //        ArmEstimator.Output armOut = armEstimator.Estimate(armIn);
            return;
        }
    }
}
