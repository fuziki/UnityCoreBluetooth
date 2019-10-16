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
    public class HumanoidController : MonoBehaviour
    {
        [SerializeField]
        private GvrArmModel armModel;

        [SerializeField]
        private Animator targetAnimator;

        [SerializeField]
        private PartsBonePair[] partsToBone = new PartsBonePair[]
        {
            new PartsBonePair(){ key = ArmModel.BodyParts.Torso, value = HumanBodyBones.Spine },
            new PartsBonePair(){ key = ArmModel.BodyParts.Shoulder, value = HumanBodyBones.RightUpperArm },
            new PartsBonePair(){ key = ArmModel.BodyParts.Elbow, value = HumanBodyBones.RightLowerArm },
            new PartsBonePair(){ key = ArmModel.BodyParts.Wrist, value = HumanBodyBones.RightHand },
        };

        private Dictionary<HumanBodyBones, Quaternion> poseBackup = new Dictionary<HumanBodyBones, Quaternion>();

        void Awake()
        {
            foreach (HumanBodyBones bone in System.Enum.GetValues(typeof(HumanBodyBones)))
            {
                if (HumanBodyBones.LastBone <= bone || this.targetAnimator.GetBoneTransform(bone) == null) continue;
                poseBackup.Add(bone, this.targetAnimator.GetBoneTransform(bone).rotation);
            }
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
            foreach(PartsBonePair pair in this.partsToBone)
            {
                Quaternion rot = GetQuaternionFromArmModel(armModel, pair.key);
                this.targetAnimator.GetBoneTransform(pair.value).rotation = rot * poseBackup[pair.value];
            }
        }

        private Quaternion GetQuaternionFromArmModel(GvrArmModel armModel, ArmModel.BodyParts parts)
        {
            Quaternion rot;
            switch (parts)
            {
                case ArmModel.BodyParts.Head:
                    rot = Quaternion.identity;
                    break;
                case ArmModel.BodyParts.Neck:
                    rot = Quaternion.identity;
                    break;
                case ArmModel.BodyParts.Torso:
                    rot = Quaternion.Euler(0, armModel.TorsoRotation.eulerAngles.y, 0);
                    break;
                case ArmModel.BodyParts.Shoulder:
                    rot = armModel.ShoulderRotation;
                    break;
                case ArmModel.BodyParts.Elbow:
                    rot = armModel.ElbowRotation;
                    break;
                case ArmModel.BodyParts.Wrist:
                    rot = armModel.WristRotation;
                    break;
                default:
                    rot = Quaternion.identity;
                    break;
            }
            return rot;
        }

    }


    [System.Serializable]
    public class DrawableKeyValuePair<TKeyType, TValueType>
    {
        public TKeyType key;
        public TValueType value;
    }

    [System.Serializable]
    public class PartsBonePair : DrawableKeyValuePair<ArmModel.BodyParts, HumanBodyBones> { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(PartsBonePair), true)]
    public class ExtensionDrawableKeyValuePair : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var defaultIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            drawProperty(position, 0.0f, 0.35f, property.FindPropertyRelative("key"));
            drawProperty(position, 0.35f, 1.0f, property.FindPropertyRelative("value"), " ->  ");


            EditorGUI.indentLevel = defaultIndentLevel;
            EditorGUI.EndProperty();
        }

        private void drawProperty(Rect beginRect, float startRate, float toRate, SerializedProperty serializedProperty, string label = "")
        {
            var width = beginRect.width * (toRate - startRate);
            var rect = new Rect(beginRect.x + beginRect.width * startRate, beginRect.y, width, beginRect.height);

            EditorGUIUtility.labelWidth = label.Length * 5f;
            EditorGUI.PropertyField(rect, serializedProperty, new GUIContent(label));
        }
    }
#endif



}

