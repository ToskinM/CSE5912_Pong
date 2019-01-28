using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePrompt : DialogueNode
{
    //public DialogueNode Parent { get; set; }
    //public List<DialogueNode> Children { get; set; }
    public string Words { get; set; }
    public bool IsOption { get; } = false;

    //public DialoguePrompt(DialogueNode parent, List<DialogueNode> children, string words) 
    //{
    //    Parent = parent;
    //    Children = children;
    //    Words = words;
    //}
    public DialoguePrompt(string words)
    {
        Words = words;
    }


    public DialogueNode GetCopy()
    {
        return new DialoguePrompt(Words); 
    }
}
