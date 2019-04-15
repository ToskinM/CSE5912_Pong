﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInspect : MonoBehaviour
{
    //public TextMeshProUGUI inventoryText;
    public GameObject InspectPanel;
    public InteractionPopup interaction; 
    //public Button InvoButton; 

    public TextMeshProUGUI charName;
    public TextMeshProUGUI descrip;
    public Image icon;

    public bool Shown = false;
    private bool buttonActive = false;
    private InspectFactory descripFactory;
    private IconFactory iconFactory;


    void Awake()
    {
        //charName = InspectPanel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        //descrip = InspectPanel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        //icon = InspectPanel.transform.GetChild(0).gameObject.GetComponent<Image>();
        descripFactory = new InspectFactory();
        iconFactory = new IconFactory(); 

        Shown = false;
   }

    void Update()
    {
        if(Shown && Input.GetKeyDown(KeyCode.X))
        {
            Hide();
        }

    }

    public void Show(string name)
    {
        interaction.NotAllowed = true; 
        InspectPanel.SetActive(true);
        GameStateController.current.PauseGame();
        Shown = true;
        charName.text = name;
        icon.sprite = iconFactory.GetIconFromName(name);
        descrip.text = descripFactory.Get(name);
        //switch (name)
        //{
        //    case Constants.AMARU_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.AMARU_ICON);
        //        descrip.text = descripFactory.GetAmaru(); 
        //        break;
        //    case Constants.NAIA_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.NAIA_ICON);
        //        descrip.text = descripFactory.GetNaia();
        //        break;
        //    case Constants.PINON_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.PINON_ICON);
        //        descrip.text = descripFactory.GetPinon();
        //        break;
        //    case Constants.SYPAVE_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.SYPAVE_ICON);
        //        descrip.text = descripFactory.GetSypave();
        //        break;
        //    case Constants.JERUTI_NAME:
        //        //icon.sprite = iconFactory.GetIcon(Constants.JERUTI_ICON);
        //        descrip.text = descripFactory.GetJeruti();
        //        break;
        //    case Constants.YSAPY_NAME:
        //        //icon.sprite = iconFactory.GetIcon(Constants.YSAPY_ICON);
        //        descrip.text = descripFactory.GetYsapy();
        //        break;
        //    case Constants.MOUSE_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.MOUSE_ICON);
        //        descrip.text = descripFactory.GetMouse();
        //        break;
        //    case Constants.CATBAT_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.CATBAT_ICON);
        //        descrip.text = descripFactory.GetCatbat();
        //        break;
        //    case Constants.ORBY_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.ORBY_ICON);
        //        descrip.text = descripFactory.GetOrby();
        //        break;
        //    case Constants.ORBYJR_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.ORBYJR_ICON);
        //        descrip.text = descripFactory.GetOrbyJr();
        //        break;
        //    case Constants.TEJU_NAME:
        //        icon.sprite = iconFactory.GetIcon(Constants.TEJU_ICON);
        //        descrip.text = descripFactory.GetTeju();
        //        break;
        //    default:
        //        break;
        //}
    }

    public void Show(string name, string description)
    {
        interaction.NotAllowed = true;
        InspectPanel.SetActive(true);
        GameStateController.current.PauseGame();
        Shown = true;
        charName.text = name;
        icon.sprite = iconFactory.GetIconFromName(name);
        descrip.text = description;
       
    }

    public void Show(string name, string creatureType, string description)
    {
        interaction.NotAllowed = true;
        InspectPanel.SetActive(true);
        GameStateController.current.PauseGame();
        Shown = true;
        charName.text = name;
        icon.sprite = iconFactory.GetIconFromName(creatureType);
        descrip.text = description;

    }

    public void Hide()
    {
        interaction.NotAllowed = false; 
        icon.sprite = null;
        charName.text = "???";
        descrip.text = "...";
        InspectPanel.SetActive(false);
        GameStateController.current.UnpauseGame();
        Shown = false;
    }




}
