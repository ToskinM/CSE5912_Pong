using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NaiaDialogueTrigger : MonoBehaviour
{
    public bool Complete { get { return complete; } }
    bool complete = false;

    GameObject panel;
    Image icon; 
    TextMeshProUGUI text;
    Button templateButton; 
    Queue<Button> buttons;
    bool active = false;
    public bool engaged = false; 
    Node dialogue;
    Node currNode;
    ICommand freezeCommand;
    string spriteFile;
    bool optionsDisplayed = false; 


    public NaiaDialogueTrigger(GameObject p, string characterSprite)
    {
        panel = p; //GameObject.Find("Dialogue Panel");
        icon = panel.transform.GetChild(0).GetComponent<Image>(); 
        text = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        buttons = new Queue<Button>();
        templateButton = panel.transform.GetChild(2).GetComponent<Button>();
        freezeCommand = new FreezeCameraCommand();
        spriteFile = characterSprite; 

        dialogue = new Node("");
        {
            string o, r; 

            {
                o = "Hi, Naia.";
                r = "You're looking off your guard.";
                dialogue.AddResponse(o, r);
                Node next1 = dialogue.GetNext(o); // gets node for r
                {
                    o = "Why would I be on my guard?";
                    r = "Around me? *grin* There's always reason to be cautious";
                    next1.AddResponse(o, r);
                }

                {
                    o = "Thank you?";
                    r = "That wasn't a compliment.";
                    next1.AddResponse(o, r);
                    Node next2 = next1.GetNext(o);
                    next2.AddNext("Heads up!");
                }

            }

            {
                o = "You ready to spar?";
                dialogue.AddResponse(o, "Always!");
            }
        }

        currNode = dialogue;
    }


    public void Update()
    {
        if (active)
        {
            text.text = currNode.Prompt();
            if(Input.GetKeyDown(KeyCode.Return) || currNode.Prompt().Equals(""))
            {
                if (!optionsDisplayed)
                {
                    displayOptions();
                    optionsDisplayed = true;
                }
                else if( !currNode.Prompt().Equals(""))
                {
                    continueToNextNode();
                    optionsDisplayed = false;
                }
            }

            if (currNode != null && !currNode.Prompt().Equals(""))
                engaged = true; 
        }
    }

    public void StartDialogue()
    {
        panel.SetActive(true); 
        icon.sprite = new IconFactory().GetIcon(spriteFile); 
        active = true;
        freezeCommand.Execute(); 
    }

    public void EndDialogue()
    {
        panel.SetActive(false);
        icon.sprite = null; 
        buttons.Clear(); 
        active = false;
        engaged = false; 
        freezeCommand.Unexecute();
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

    private void displayOptions()
    {
        if (currNode.HasOptions())
        {
            int currOffset = 0;
            int offset = Screen.height / 19;
            for (int i = 0; i < currNode.Options().Count; i++)
            {
                Button b = Instantiate(templateButton, templateButton.transform.position, templateButton.transform.rotation);
                b.transform.SetParent(panel.transform, false);
                b.transform.position = new Vector3(templateButton.transform.position.x, templateButton.transform.position.y - currOffset);
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
    }

}








