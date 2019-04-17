using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIconDisplay : MonoBehaviour
{
    Sprite AnaiIcon;
    Sprite MimbiIcon;

    GameObject ActiveIcon;
    GameObject CompanionIcon;

    // Start is called before the first frame update
    void Start()
    {
        IconFactory iconFactory = new IconFactory();
        AnaiIcon = new IconFactory().GetIcon(Constants.ANAI_ICON);
        MimbiIcon = new IconFactory().GetIcon(Constants.MIMBI_ICON);

        ActiveIcon = GameObject.Find("Active Player Icon");
        CompanionIcon = GameObject.Find("Companion Icon");

        if (PlayerController.instance.AnaiIsActive())
        {
            ActiveIcon.GetComponent<Image>().sprite = AnaiIcon;
            CompanionIcon.GetComponent<Image>().sprite = MimbiIcon;
        }
        else
        {
            ActiveIcon.GetComponent<Image>().sprite = MimbiIcon;
            CompanionIcon.GetComponent<Image>().sprite = AnaiIcon;
        }

    }

    void OnEnable()
    {
        PlayerController.OnCharacterSwitch += SwapPortraits;
    }

    void OnDisable()
    {
        PlayerController.OnCharacterSwitch -= SwapPortraits;
    }

    void SwapPortraits(PlayerController.PlayerCharacter activeChar)
    {
        if (activeChar == PlayerController.PlayerCharacter.Anai)
        {
            ActiveIcon.GetComponent<Image>().sprite = AnaiIcon;
            CompanionIcon.GetComponent<Image>().sprite = MimbiIcon;
        }
        else
        {
            ActiveIcon.GetComponent<Image>().sprite = MimbiIcon;
            CompanionIcon.GetComponent<Image>().sprite = AnaiIcon;
        }
    }
}
