using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;


namespace DS.Elements
{
    using Enumerations;
    using UnityEngine.UIElements;

    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialogueName";
            Choices = new List<string>();
            Text = "Dialogue Text.";

            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual void Draw()
        {
            TextField dialogueNameField = new TextField() { value = DialogueName };
            titleContainer.Insert(0, dialogueNameField);

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.name = "Dialogue Connection";
            inputContainer.Add(inputPort);

            VisualElement customDataContainer = new VisualElement();
            Foldout textFoldout = new Foldout() { text = "Dialogue Text"};
            TextField textText = new TextField() { value = Text };
            textFoldout.Add(textText);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }
    }
}
