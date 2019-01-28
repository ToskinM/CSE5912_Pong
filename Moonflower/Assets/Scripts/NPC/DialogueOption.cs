﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOption : DialogueNode
{
    //public DialogueNode Parent { get; set; }
    //public List<DialogueNode> Children { get; set; }
    public string Words { get; set; }
    public bool IsOption { get; } = true;


    //public DialogueOption(DialogueNode parent, List<DialogueNode> children, string words)
    //{
    //    Parent = parent;
    //    Children = children;
    //    Words = words;
    //}

    public DialogueOption(string words)
    {
        Words = words;
    }

    public DialogueNode GetCopy()
    {
        return new DialogueOption(Words);
    }

}
