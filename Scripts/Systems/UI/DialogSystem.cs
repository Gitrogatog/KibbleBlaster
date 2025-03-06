namespace MyECS.Systems;
using System;
using Godot;
using MoonTools.ECS;
using MyECS.Components;
using MyECS.Messages;

public class DialogSystem : System
{
    RichTextLabel textNode;
    string currentText;
    int currentCharacter = 0;
    float currentTextTime = 0;
    float timeBetweenCharacters = 0.2f;

    public DialogSystem(World world, RichTextLabel textNode) : base(world)
    {
        this.textNode = textNode;
        textNode.VisibleCharacters = 0;
        textNode.Text = "hello\nthere";
    }
    public override void Update(TimeSpan delta)
    {

        foreach (DisplayDialogMessage dialogMessage in ReadMessages<DisplayDialogMessage>())
        {
            currentText = TextStorage.GetString(dialogMessage.ID);
            currentCharacter = 0;
            textNode.Text = currentText;
            // textNode.
        }
        currentTextTime += (float)delta.TotalSeconds;
        if (currentTextTime >= timeBetweenCharacters)
        {
            currentTextTime -= timeBetweenCharacters;
            currentCharacter++;
        }
    }

    enum DialogChar
    {
        Default,
        Period,

    }
}