using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOption 
{
    public string Response { get; set; }
    public int DestNodeID { get; set; }

    public DialogueOption() { }

    public DialogueOption(string resp, int destID)
    {
        Response = resp; 
        DestNodeID = destID; 
    }
}
