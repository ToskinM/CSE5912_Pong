using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dialogue; 

public class EngagementOptionsController : MonoBehaviour
{
    public enum EngageType { talk, distract, give }

    public delegate void Function();

    public GameObject panel; 
    public GameObject TalkButton;
    public GameObject DistractButton;
    public GameObject GiveButton;
    public Image icon;

    private ICommand freezeCommand;
    public bool Showing = false;
    private bool cameraFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        freezeCommand = new FreezeCameraCommand();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void updateDisplay()
    {
        if (!TalkButton.active && !DistractButton.active)
        {
            oneButton(GiveButton);
        }
        else if (!DistractButton.active && !GiveButton.active)
        {
            oneButton(TalkButton);
        }
        else if (!GiveButton.active && !TalkButton.active)
        {
            oneButton(DistractButton);
        }
        else if (!TalkButton.active)
        {
            twoButtons(DistractButton, GiveButton);
        }
        else if (!DistractButton.active)
        {
            twoButtons(TalkButton, GiveButton);
        }
        else if (!GiveButton.active)
        {
            twoButtons(TalkButton, DistractButton);
        }
    }

    public Button GetButton(EngageType type)
    {
        Button b = null; 
        switch (type)
        {
            case EngageType.talk:
                b = TalkButton.GetComponent<Button>();
                break;
            case EngageType.distract:
                b = DistractButton.GetComponent<Button>();
                break;
            case EngageType.give:
                b = GiveButton.GetComponent<Button>();
                break;
        }
        b.gameObject.SetActive(true);
        return b; 
    }

    public void EnablePanel(string characterSprite)
    {
        Showing = true;
        panel.SetActive(true);
        icon.sprite = new IconFactory().GetIcon(characterSprite);

        freezeCommand.Execute(); 
    }

    public void DisablePanel()
    {
        Showing = false; 
        panel.SetActive(false);
        TalkButton.SetActive(false);
        DistractButton.SetActive(false);
        GiveButton.SetActive(false);
        freezeCommand.Unexecute(); 
    }

    private void twoButtons(GameObject a, GameObject b)
    {
        float center = panel.transform.position.x; 
        float x = Screen.width / 4.5f;
        float y = a.transform.position.y;
        a.transform.position = new Vector3(center - x, y);
        b.transform.position = new Vector3(center + x, y);  
    }

    private void oneButton(GameObject a)
    {
        float center = panel.transform.position.x;
        float y = a.transform.position.y;
        a.transform.position = new Vector3(center, y);
    }
}
