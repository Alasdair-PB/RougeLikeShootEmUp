using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using UnityEngine;

namespace DS.Windows
{
    using Elements;
    using Enumerations;

    public class DSGraphView : GraphView
    {
        public DSGraphView() {
            AddManipulators();
            AddGridBackground();

            //CreateNode();
            AddStyles();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Single Choice Node", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Multiple Choice Node", DSDialogueType.MultipleChoice));

        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent =>
                AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
                ); 
            return contextualMenuManipulator;
        }

        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");
            DSNode node = (DSNode) Activator.CreateInstance(nodeType);
            node.Initialize(position);
            node.Draw();
            return node;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("GraphTools/DSGraphViewStyles.uss");
            styleSheets.Add(styleSheet);
        }
    }

}
