using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmaruDialogueTrigger : MonoBehaviour
{
    GameObject panel;
    TextMeshProUGUI text;
    bool active = false;
    Node dialogue;
    Node currNode;

    // Start is called before the first frame update

    public AmaruDialogueTrigger(GameObject p, TextMeshProUGUI t)
    {
        panel = p; //GameObject.Find("Dialogue Panel");
        text = t; //GameObject.Find("Dialogue Text").GetComponent<TextMeshProUGUI>();
        dialogue = new Node("Hello");
        string s = "Hi!";
        dialogue.AddOption(s);
        dialogue.AddResponse(s, "I'm glad life is chill");
        Node next = dialogue.GetNext(s);
        next.AddNext("Glad meeting you!");
        s = "Fuck off";
        dialogue.AddOption(s);
        dialogue.AddResponse(s, "Well that's rude");
        next = dialogue.GetNext(s);
        next.AddNext("Fuck you too I guess");

        currNode = dialogue;
    }

    // Update is called once per frame

    public void Update()
    {
        if (active)
        {
            text.text = currNode.Prompt();
            if (currNode.HasOptions())
            {
                //int i = 0;
                //foreach (string op in currNode.Options())
                //{
                //    //make buttons
                //    GUI.Button(new Rect(10, 10, panel.transform.position.x, panel.transform.position.y + i), op);
                //    i += 1;
                //}
            }
        }
    }

    public void StartDialogue()
    {
        panel.SetActive(true);
        active = true;
    }

    public void EndDialogue()
    {
        panel.SetActive(false);
        active = false;
    }

    public bool DialogueActive()
    {
        return active;
    }

}








