using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DialogueNode 
{
    //DialogueNode Parent { get; set; } 
    //List<DialogueNode> Children { get; set; }
    string Words { get; set; }
    bool IsOption { get; }

    DialogueNode GetCopy(); 

}