using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    public GameObject dropDownMenuObject;
    public Button dropDownMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        dropDownMenuButton.onClick.AddListener(AbleDropDownMenu);
        
    }


    public void AbleDropDownMenu()
    {
        dropDownMenuObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
