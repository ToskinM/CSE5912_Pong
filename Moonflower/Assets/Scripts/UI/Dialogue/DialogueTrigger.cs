using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dialogue; 

public class DialogueTrigger : MonoBehaviour
{
    public bool Complete { get { return complete; } }
    bool complete = false;

    GameObject panel;
    Image icon; 
    TextMeshProUGUI text;
    Button templateButton;
    ICommand freezeCommand;

    Queue<Button> buttons;
    string spriteFile;

    bool active = false;
    public bool engaged = false; 
    bool optionsDisplayed = false;

    DialogueGraph graph;
    DialogueFactory factory; 

    public DialogueTrigger(GameObject p, string characterSprite, string graphName)
    {
        factory = new DialogueFactory();
        graph = factory.GetDialogue(graphName);
        graph.Restart(); 
        panel = p;
        icon = panel.transform.GetChild(0).GetComponent<Image>(); 
        text = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        buttons = new Queue<Button>();
        templateButton = panel.transform.GetChild(2).GetComponent<Button>();
        freezeCommand = new FreezeCameraCommand();
        spriteFile = characterSprite; 
    }


    public void Update()
    {
        if (active)
        {
            text.text = graph.current.text;
            if (Input.GetKeyDown(KeyCode.Return) || text.text.Equals(""))
            {
                if (!optionsDisplayed)
                {
                    displayOptions();
                    optionsDisplayed = true;
                }
                else if( !text.text.Equals(""))
                {
                    gotoNext(-1);
                    optionsDisplayed = false;
                }
                engaged = true; 
            }

            //if (currNode != null && !currNode.Prompt().Equals(""))
                //engaged = true; 
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

    private void gotoNext(int i)
    {
        string prev = graph.current.text; 
        graph.GetNext(i);
        string next = graph.current.text;
        if (next.Equals(prev))
        {
            EndDialogue();
            complete = true; 
        }
        destroyButtons();
    }

    private void displayOptions()
    {
        int numOptions = graph.current.answers.Count;
        if (numOptions > 0)
        {
            int currOffset = 0;
            int offset = Screen.height / 19;
            for (int i = 0; i < numOptions; i++)
            {
                Button b = Instantiate(templateButton, templateButton.transform.position, templateButton.transform.rotation);
                b.transform.SetParent(panel.transform, false);
                b.transform.position = new Vector3(templateButton.transform.position.x, templateButton.transform.position.y - currOffset);
                currOffset += offset;

                buttons.Enqueue(b);
            }
            for (int i = 0; i < numOptions; i++)
            {
                Chat.Answer answer = graph.current.answers[i];
                Button currButton = buttons.Dequeue();
                currButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer.text; //op;
                currButton.onClick.AddListener(delegate { gotoNext(graph.current.answers.IndexOf(answer)); });
                buttons.Enqueue(currButton);
            }
        }
    }

}








