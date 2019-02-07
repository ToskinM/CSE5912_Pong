using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmaruDialogueTrigger : MonoBehaviour
{
    public bool Complete { get { return complete; } }
    bool complete = false;

    GameObject panel;
    TextMeshProUGUI text;
    Button templateButton; 
    Queue<Button> buttons;
    bool active = false;
    Node dialogue;
    Node currNode;
    ICommand freezeCommand; 


    public AmaruDialogueTrigger(GameObject p)
    {
        panel = p; //GameObject.Find("Dialogue Panel");
        text = panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        buttons = new Queue<Button>();
        templateButton = panel.transform.GetChild(1).GetComponent<Button>();
        freezeCommand = new FreezeCameraCommand();  

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


    public void Update()
    {
        if (active)
        {
            text.text = currNode.Prompt();
            if (currNode.HasOptions() && buttons.Count == 0)
            {
                int currOffset = 0;
                int offset = 12;
                int sideMargin = 25;
                int topMargin = 5; 
                for (int i = 0; i < currNode.Options().Count; i++)
                {
                    Button b = Instantiate(templateButton, templateButton.transform.position, templateButton.transform.rotation);
                    b.transform.SetParent(panel.transform,false);
                    b.transform.position = new Vector3(b.transform.position.x - (Screen.width/2.0f) + sideMargin, b.transform.position.y - currOffset - topMargin);
                    currOffset += offset; 

                    buttons.Enqueue(b); 
                }
                foreach (string op in currNode.Options())
                {
                    Button currButton = buttons.Dequeue();
                    currButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = op;
                    currButton.onClick.AddListener(delegate { changeCurrNode(op); });
                    buttons.Enqueue(currButton); 
                }
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                continueToNextNode(); 
            }
        }
    }

    public void StartDialogue()
    {
        panel.SetActive(true);
        active = true;
        freezeCommand.Execute(); 
    }

    public void EndDialogue()
    {
        panel.SetActive(false);
        buttons.Clear(); 
        active = false;
        freezeCommand.Execute();
    }

    public bool DialogueActive()
    {
        return active;
    }

    private void destroyButtons()
    {
        while(buttons.Count > 0)
        {
            Button b = buttons.Dequeue(); 
            Destroy(b.gameObject); 
        }
    }

    private void changeCurrNode(string s)
    {
        currNode = currNode.GetNext(s);
        destroyButtons(); 
    }

    private void continueToNextNode()
    {
        if(!currNode.HasOptions())
        {
            currNode = currNode.GetNext();
            if (currNode == null)
            {
                EndDialogue();
                complete = true; 
            }
        }
    }

}








