using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{
    public int NodeID { get; set; }
    public List<DialogueOption> Options { get; set; }
    public string prompt; 

    public DialogueNode() { }

    public DialogueNode(int id, List<DialogueOption> opts)
    {
        NodeID = id;
        Options = opts;
    }

}
