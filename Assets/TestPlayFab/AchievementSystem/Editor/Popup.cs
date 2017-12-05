using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Achievements
{
    public class PopUp : EditorWindow
    {
        public static string content = "";

        public  void Init(string _content, Rect rect)
        {
            PopUp window = ScriptableObject.CreateInstance<PopUp>();
            window.position = rect;
            content = _content;
            window.ShowPopup();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField(content, EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("OK !")) this.Close();
        }

        public void Display(string _content, Rect rect)
        {
            Init(_content, rect);
            OnGUI();
        }

        public void OnLostFocus()
        {
            this.Close();
        }
    }
}
