using System.Collections;
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

        [SerializeField]
        public KeyValuePair<string, string> dic;
 


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

[CustomPropertyDrawer(typeof(Hakumuchu.HumanoidController))]
public class ElementBehaviourDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 普通にプロパティを描画
        using (new EditorGUI.PropertyScope(position, label, property))
        {
            EditorGUI.PropertyField(position, property);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}