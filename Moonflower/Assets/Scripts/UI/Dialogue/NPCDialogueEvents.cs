using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events; 
using TMPro;


public class NPCDialogueEvents : MonoBehaviour
{
    public static NPCDialogueEvents instance;

    void Start()
    {
        instance = this; 
    }

    const int playerStatBuff = 3;

    public void InduceFight(string NPCname)
    {
        switch (NPCname)
        {
            case "Naia":
                GameObject.Find("Naia").GetComponent<NaiaController>().Fight();
                break;
            default:
                break;
        }
    }

    public void IncreasePlayerCharisma(bool pos)
    {
        if (pos)
            PlayerController.instance.gameObject.GetComponent<CharacterStats>().Charisma += playerStatBuff;
        else
            PlayerController.instance.gameObject.GetComponent<CharacterStats>().Charisma -= playerStatBuff;
    
    }

    public void IncreasePlayerCunning(bool pos)
    {
        if (pos)
            PlayerController.instance.gameObject.GetComponent<CharacterStats>().Cunning += playerStatBuff;
        else
            PlayerController.instance.gameObject.GetComponent<CharacterStats>().Cunning -= playerStatBuff;

    }

    public void GiveToPlayer(string giftName)
    {
        displayFeedback("You've been given a " + giftName.ToLower() + "!");
        PlayerController.instance.gameObject.GetComponent<PlayerInventory>().AddObj(giftName);
    }

    private void displayFeedback(string text)
    {
        GameObject.Find("FeedbackText").GetComponent<FeedbackText>().ShowText(text); 
    }
}
