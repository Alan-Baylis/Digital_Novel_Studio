﻿using UnityEditor;
using UnityEngine;

namespace XNodeEditor
{
    [CustomNodeEditor(typeof(CommentNode))]
    public class CommentEditor : NodeEditor
    {
        private bool _onError = false;
        private CommentNode _commentNode;
        private bool toggle;
        
        public override void OnHeaderGUI()
        {
            //toggle = GUILayout.Toggle(toggle, "");
            GUILayout.Label(target.name, NodeEditorResources.styles.nodeHeader, GUILayout.Height(30));
        }

        public override void OnBodyGUI()
        {
            if (_commentNode == null)
            {
                _commentNode = target as CommentNode;
            }

            serializedObject.Update();

            GUILayout.BeginVertical();
            GUILayout.Space(90);
            GUILayout.EndVertical();
            GUIStyle tempStyle = new GUIStyle();
            tempStyle.normal.background = RenpyMaker.MakeTex(190, 90, new Color(1f, 1f, 0.8f), new RectOffset(0, 0, 0, 0), Color.black); 
            EditorStyles.textField.wordWrap = true;
            _commentNode.comment = EditorGUI.TextArea(new Rect(6, 30, 196, 90), _commentNode.comment, tempStyle);

            NodeEditorGUILayout.PortPair(_commentNode.GetInputPort("entry"), _commentNode.GetOutputPort("exit"));

            serializedObject.ApplyModifiedProperties();
        }

        public override Color GetTint()
        {
            SerializedProperty errorProp = serializedObject.FindProperty("errorStatus");
            _onError = errorProp.boolValue;

            if (_onError)
                return new Color(0.5f, 0, 0);
            else
                return NodeEditorPreferences.GetSettings().tintColor;
        }
    }
}