using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hakumuchu
{
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
