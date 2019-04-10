using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePanelInfo : MonoBehaviour
{
    public Vector3 UpPosition;
    public Vector3 DownPosition;
    public GameObject panel; 
    public Image Icon;
    public TextMeshProUGUI Text;
    public Button TemplateButton;
    public bool IsUp = true;

    // Start is called before the first frame update
    void Start()
    {
        if(Icon==null)
            Icon = panel.transform.GetChild(0).GetComponent<Image>();
        if(Text==null)
            Text = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if(TemplateButton==null)
            TemplateButton = transform.GetChild(2).GetComponent<Button>();

        UpPosition = panel.transform.position;
        DownPosition = new Vector3(UpPosition.x, UpPosition.y - Screen.height/2, UpPosition.z);
        panel.transform.position = DownPosition;
    }


}

