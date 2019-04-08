using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueController
{
    bool DialogueActive { get; set; }

    void Talk();
    void StartTalk();
    void EndTalk(); 
}