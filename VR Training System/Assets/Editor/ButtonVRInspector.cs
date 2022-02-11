using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;

public class ButtonVRInspector : Button
{
    // Since we are inheriting form Button new properties are not shown in inspector --> add event manually
    [CustomEditor(typeof(ButtonVR))]
    public class MenuButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            SerializedProperty onPressed = serializedObject.FindProperty("onPressed"); // <-- UnityEvent
            //EditorGUILayout.PropertyField(onPressed);
            //serializedObject.ApplyModifiedProperties();           
        }
    }
}
