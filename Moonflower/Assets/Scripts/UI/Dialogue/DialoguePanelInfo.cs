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
    public bool IsUp = true;

    // Start is called before the first frame update
    void Start()
    {

        Icon = transform.GetChild(0).GetComponent<Image>();
        Text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TemplateButton = transform.GetChild(2).GetComponent<Button>();

        UpPosition = gameObject.transform.position;
        DownPosition = new Vector3(UpPosition.x, UpPosition.y - Icon.rectTransform.rect.height*2, UpPosition.z);
        transform.position = DownPosition;
    }


}

