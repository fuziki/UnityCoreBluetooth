using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hakumuchu
{
    public class HumanoidController : MonoBehaviour
    {
        public enum BodyPartsType
        {
            Head, Neck, Torso, Shoulder, Elbow, Wrist
        }

        [System.Serializable]
        public class BodyPartsConfig
        {
            public BodyPartsType type;
            public HumanBodyBones targetBone;
            public BodyPartsConfig(BodyPartsType type, HumanBodyBones targetBone)
            {
                this.type = type; this.targetBone = targetBone;
            }
        }

        public GameObject targetHumanoid;

        [SerializeField]
        public BodyPartsConfig[] configs = new BodyPartsConfig[] 
        {
            new BodyPartsConfig(BodyPartsType.Torso, HumanBodyBones.Spine),
            new BodyPartsConfig(BodyPartsType.Shoulder, HumanBodyBones.RightUpperArm),
            new BodyPartsConfig(BodyPartsType.Elbow, HumanBodyBones.RightLowerArm),
            new BodyPartsConfig(BodyPartsType.Wrist, HumanBodyBones.RightHand),
        };


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

