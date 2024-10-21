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

            mainContainer.AddToClassList("ds-node_main-container");
            extensionContainer.AddToClassList("ds-node_extension-container");
        }

        public virtual void Draw()
        {
            TextField dialogueNameField = new TextField() { value = DialogueName };
            dialogueNameField.AddToClassList("ds-node_textfield");
            dialogueNameField.AddToClassList("ds-node_filename-textfield");
            dialogueNameField.AddToClassList("ds-node_textfield_hidden");


            titleContainer.Insert(0, dialogueNameField);

            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.name = "Dialogue Connection";
            inputContainer.Add(inputPort);

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node_custom-data-container");
            Foldout textFoldout = new Foldout() { text = "Dialogue Text"};

            TextField textText = new TextField() { value = Text };
            textText.AddToClassList("ds-node_textfield");
            textText.AddToClassList("ds-node_quote-textfield");


            textFoldout.Add(textText);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);
        }
    }
}
