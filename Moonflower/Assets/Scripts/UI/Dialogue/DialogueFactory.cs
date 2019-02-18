using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dialogue;

public class DialogueFactory
{

    public DialogueGraph GetDialogue(string file)
    {
        return Resources.Load<DialogueGraph>(file);
    }
}
