using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EngagementOptionsController : MonoBehaviour
{
    public GameObject TalkButton;
    public GameObject DistractButton;
    public GameObject GiveButton; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("did the thing"); 
                DisablePanel();
            }
        }
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

    private void DisablePanel()
    {
        
    }

    private void twoButtons(GameObject a, GameObject b)
    {
        float center = transform.position.x; 
        float x = Screen.width / 4.5f;
        float y = a.transform.position.y;
        a.transform.position = new Vector3(center - x, y);
        b.transform.position = new Vector3(center + x, y);  
    }

    private void oneButton(GameObject a)
    {
        float center = transform.position.x;
        float y = a.transform.position.y;
        a.transform.position = new Vector3(center, y);
    }
}
