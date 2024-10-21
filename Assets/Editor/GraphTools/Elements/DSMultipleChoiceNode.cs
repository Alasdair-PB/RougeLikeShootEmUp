using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using UnityEngine.UIElements;

    public class DSMultipleChoiceNode : DSNode
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            DialogueType = Enumerations.DSDialogueType.MultipleChoice;
            Choices.Add("New Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            Button addChoiceButton = new Button() { text = "Add choice"};
            addChoiceButton.AddToClassList("ds-node_button");
            mainContainer.Insert(1, addChoiceButton);

            foreach (var choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = "";

                Button deleteChoiceButton = new Button() { text = "X" };
                deleteChoiceButton.AddToClassList("ds-node_button");


                TextField choiceTextField = new TextField() { value = choice};
                choiceTextField.style.flexDirection = FlexDirection.Column;

                choiceTextField.AddToClassList("ds-node_textfield");
                choiceTextField.AddToClassList("ds-node_choice-textfield");
                choiceTextField.AddToClassList("ds-node_textfield_hidden");


                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}
