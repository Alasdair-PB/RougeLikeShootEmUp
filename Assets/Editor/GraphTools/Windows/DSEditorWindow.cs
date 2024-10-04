using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void CreateGUI()
        {
            AddGraphView();
            AddStyles();
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("GraphTools/DSVariables.uss");
            rootVisualElement.styleSheets.Add(styleSheet);
        }

        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
    }
}
