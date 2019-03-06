using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePanelInfo : MonoBehaviour
{
    public Vector3 UpPosition;
    public Vector3 DownPosition; 
    public Image Icon;
    public TextMeshProUGUI Text;
    public Button TemplateButton;

    // Start is called before the first frame update
    void Awake()
    {
        //Icon = transform.GetChild(0).GetComponent<Image>();
        //Text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //TemplateButton = transform.GetChild(2).GetComponent<Button>();
        UpPosition = transform.position;
        DownPosition = new Vector3(UpPosition.x, UpPosition.y - Icon.rectTransform.rect.height);

        transform.position = DownPosition; 
    }

}
