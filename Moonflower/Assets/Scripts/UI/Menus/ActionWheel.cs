using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionWheel : MonoBehaviour
{
    public Image icon;

    //public Button inspectButton;
    //public Button talkButton;
    //public Button distractButton;
    //public Button giveButton;
    [Header("Inspect, Talk, Distract, Give")]
    public Button[] buttons;

    public delegate void SelectionUpdate(int buttonIndex);
    public event SelectionUpdate OnSelectOption;

    private void Start()
    {
        //for (int i = 0; i < 4; i++)
        //{
        //    buttons[i].onClick.AddListener(() => OnButtonClick(i));
        //}

        buttons[0].onClick.AddListener(() => OnButtonClick(0));
        buttons[1].onClick.AddListener(() => OnButtonClick(1));
        buttons[2].onClick.AddListener(() => OnButtonClick(2));
        buttons[3].onClick.AddListener(() => OnButtonClick(3));

        //inspectButton.onClick.AddListener(() => OnButtonClick(0));
        //talkButton.onClick.AddListener(() => OnButtonClick(1));
        //distractButton.onClick.AddListener(() => OnButtonClick(2));
        //giveButton.onClick.AddListener(() => OnButtonClick(3));
    }

    private void OnButtonClick(int buttonIndex)
    {
        OnSelectOption?.Invoke(buttonIndex);
    }

    public void Initialize(Sprite iconSprite, bool[] actionsAvailable)
    {
        icon.sprite = iconSprite;

        for (int i = 0; i < 4; i++)
        {
            buttons[i].transform.parent.gameObject.SetActive(actionsAvailable[i]);
        }
    }

    void Update()
    {
        
    }
}
