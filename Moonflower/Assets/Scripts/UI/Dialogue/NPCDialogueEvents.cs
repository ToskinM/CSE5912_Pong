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

    const int playerStatBuff = 1;

    public void InduceFight(string NPCname)
    {
        switch (NPCname)
        {
            case "Naia":
                    Invoke("fight", 2);
                break;
            default:
                break;
        }
    }

    public void TejuFail()
    {
        GameObject.Find("Teju").GetComponent<TejuController>().FailConvo();
        IncreasePlayerCharisma(false); 
    }

    public void TejuSuccess()
    {
        GameObject.Find("Teju").GetComponent<TejuController>().Subdue();
        IncreasePlayerCharisma(true);
    }

    public bool WasMorePeaceful()
    {
        return PlayerController.instance.ActivePlayerStats.Charisma > PlayerController.instance.ActivePlayerStats.Strength;
    }

    public void MoveMouse()
    {
        GameObject.Find("BigMouse").GetComponent<TuvichaDialogueController>().Move();
    }

    private void fight()
    {
        GameObject.Find("Naia").GetComponent<NaiaController>().Fight();
    }

    public void IncreasePlayerCharisma(bool pos)
    {
        if (pos)
            PlayerController.instance.gameObject.GetComponent<CharacterStats>().Charisma += playerStatBuff;
        else
            PlayerController.instance.gameObject.GetComponent<CharacterStats>().Charisma -= playerStatBuff;
    
    }

    public void GiveToPlayer(string giftName)
    {
        displayFeedback("You've been given a " + giftName.ToLower() + "!");
        if(giftName.Equals(Constants.MOONFLOWER_NAME))
        {
            bool added = PlayerController.instance.ActivePlayerStats.AddHealth(10);
            if(!added)
            {
                PlayerController.instance.gameObject.GetComponent<PlayerInventory>().AddObj(giftName);
            }
        }
        else
        {
            PlayerController.instance.gameObject.GetComponent<PlayerInventory>().AddObj(giftName);
        }
    }

    private void displayFeedback(string text)
    {
        GameObject.Find("FeedbackText").GetComponent<FeedbackText>().ShowText(text); 
    }
}
