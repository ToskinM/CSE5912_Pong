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

    enum TextState { typing, paused, options, done}
    TextState state = TextState.done; 
    bool typing = false;
    int typeIndex = 0;

    int slowDownFrac = 2;
    int pauseCount = 0;
    int pauseMax = 10;
    string punctuation = ".!?"; 

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
            switch(state)
            {
                case TextState.typing:
                case TextState.paused:
                    typeText();
                    break;
                case TextState.options:
                    displayOptions();
                    break; 
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("what state? " + state); 
                switch(state)
                {
                    case TextState.typing:
                    case TextState.paused:
                        text.text = graph.current.text;
                        typeIndex = 0; 
                        state = TextState.options; 
                        break;
                    case TextState.options:
                        displayOptions();
                        state = TextState.done; 
                        break;
                    case TextState.done:
                        if (!hasOptions())
                        {
                            gotoNext(-1);
                        }
                        break;
                }
                engaged = true; 
            }
        }
    }

    public void StartDialogue()
    {
        panel.SetActive(true); 
        icon.sprite = new IconFactory().GetIcon(spriteFile); 
        active = true;
        state = TextState.typing; 
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

    private void typeText()
    {
        string dialogue = graph.current.text; 
        switch(state)
        {
            case TextState.typing:
                if (typeIndex%slowDownFrac == 0)
                    text.text = dialogue.Substring(0, typeIndex/slowDownFrac);

                typeIndex++;
                int currDiaIndex = typeIndex / slowDownFrac;
                //if (currDiaIndex == dialogue.Length)
                //{
                //    typeIndex = 0;
                //    typing = false;
                //    state = TextState.options;  
                //}
                if(currDiaIndex > 1 && punctuation.IndexOf(dialogue[currDiaIndex-2]) != -1)
                    state = TextState.paused;
                break;

            case TextState.paused:
                pauseCount++; 
                if(pauseCount == pauseMax)
                {
                    pauseCount = 0;
                    if (typeIndex / slowDownFrac == dialogue.Length)
                    {
                        typeIndex = 0;
                        typing = false;
                        if (hasOptions())
                            state = TextState.options;
                        else
                            state = TextState.done; 
                    }
                    else
                    {
                        state = TextState.typing;
                    }
                }
                break;
            default:
                break;
        }
    }

    private bool hasOptions()
    {
        return graph.current.answers.Count > 0;
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
        state = TextState.typing; 
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
            int offset = Screen.height / 21;
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
        state = TextState.done; 
    }

}








