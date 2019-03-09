using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionWheel : MonoBehaviour
{
    public Image icon;

    public Button inspectButton;
    public Button talkButton;
    public Button distractButton;
    public Button giveButton;

    public delegate void SelectionUpdate(int buttonIndex);
    public event SelectionUpdate OnSelectOption;

    private void Start()
    {
        inspectButton.onClick.AddListener(() => OnButtonClick(0));
        talkButton.onClick.AddListener(() => OnButtonClick(1));
        distractButton.onClick.AddListener(() => OnButtonClick(2));
        giveButton.onClick.AddListener(() => OnButtonClick(3));
    }

    private void OnButtonClick(int buttonIndex)
    {
        OnSelectOption?.Invoke(buttonIndex);
    }

    public void Initialize(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    void Update()
    {
        
    }
}
