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
    Button exitButton; 
    ICommand freezeCommand;

    List<Button> buttons;
    string spriteFile;
    string exitText = "Okay. See you around!"; 

    public bool engaged = false; 

    enum PanelState { down, rising, up, falling}
    PanelState pState = PanelState.down;
    enum TextState { typing, paused, options, done, ending}
    TextState tState = TextState.done; 
    int typeIndex = 0;
    const int fadeMax = 30;
    Button currB;
    Vector3 upPos;
    Vector3 downPos;  

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
        buttons = new List<Button>(); 
        templateButton = panel.transform.GetChild(2).GetComponent<Button>();
        exitButton = panel.transform.GetChild(3).GetComponent<Button>();
        exitButton.onClick.AddListener(endConvo);

        freezeCommand = new FreezeCameraCommand();
        spriteFile = characterSprite;
        upPos = panel.transform.position;
        downPos = new Vector3(upPos.x, upPos.y - icon.rectTransform.rect.height);
        panel.transform.position = downPos; 
    }

    private void ButtonClicked(int n)
    {
        Debug.Log("Button clicked = " + n);
    }

    public void Update()
    {
        if (pState != PanelState.down)
        {
            switch (pState)
            {
                case PanelState.rising:
                    panel.transform.position += new Vector3(0, 4); 
                    if(panel.transform.position.y >= upPos.y)
                    {
                        panel.transform.position = upPos; 
                        pState = PanelState.up; 
                    }
                    break;

                case PanelState.falling:
                    panel.transform.position -= new Vector3(0, 4);
                    if (panel.transform.position.y <= downPos.y)
                    {
                        pState = PanelState.down;
                        panel.SetActive(false);
                    }
                    break;

                case PanelState.up:
                    switch (tState)
                    {
                        case TextState.ending:
                            typeEnding(); 
                            break;
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
                        switch (tState)
                        {
                            case TextState.ending:
                                if(!text.text.Equals(exitText))
                                {
                                    text.text = graph.current.text;
                                }
                                else
                                {
                                    pState = PanelState.down;
                                }
                                break; 
                            case TextState.typing:
                            case TextState.paused:
                                text.text = graph.current.text;
                                typeIndex = 0;
                                tState = TextState.options;
                                break;
                            case TextState.options:
                                forceOptions();
                                tState = TextState.done;
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
                    break; 

            }
        }
    }

    public void StartDialogue()
    {
        pState = PanelState.rising; 
        panel.SetActive(true); 
        icon.sprite = new IconFactory().GetIcon(spriteFile); 
        tState = TextState.typing; 
        freezeCommand.Execute();
    }

    public void EndDialogue()
    {
        //panel.SetActive(false);
        destroyButtons(); 
        //active = false;
        pState = PanelState.falling; 
        engaged = false;
        freezeCommand.Unexecute();
    }

    public bool DialogueActive()
    {
        return pState != PanelState.down;
    }

    public void SetExitText(string s)
    {
        exitText = s;
    }

    private void endConvo()
    {
        Debug.Log("I got clicked!"); 
        tState = TextState.ending;
        typeIndex = 0; 
    }

    private void typeEnding()
    {
        if (typeIndex % slowDownFrac == 0)
            text.text = exitText.Substring(0, typeIndex / slowDownFrac);

        typeIndex++;
        int currDiaIndex = typeIndex / slowDownFrac;
        //if (currDiaIndex == dialogue.Length)
        //{
        //    typeIndex = 0;
        //    typing = false;
        //    state = TextState.options;  
        //}
        if (currDiaIndex > 1 && punctuation.IndexOf(exitText[currDiaIndex - 2]) != -1)
            tState = TextState.paused;
    }

    private void typeText()
    {
        string dialogue = graph.current.text; 
        switch(tState)
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
                    tState = TextState.paused;
                break;

            case TextState.paused:
                pauseCount++; 
                if(pauseCount == pauseMax)
                {
                    pauseCount = 0;
                    if (typeIndex / slowDownFrac == dialogue.Length)
                    {
                        resetCounts(); 
                        if (hasOptions())
                            tState = TextState.options;
                        else
                            tState = TextState.done; 
                    }
                    else
                    {
                        tState = TextState.typing;
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
            Button b = buttons[0];
            buttons.RemoveAt(0); 
            Destroy(b.gameObject); 
        }
        buttons.Clear(); 
    }

    private void resetCounts()
    {
        typeIndex = 0;
        pauseCount = 0;
    }

    private void gotoNext(int i)
    {
        tState = TextState.typing; 
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
            if (buttons.Count != numOptions)
            {
                int currOffset = 0;
                int offset = Screen.height / 21;
                for (int i = 0; i < numOptions; i++)
                {
                    Button b = Instantiate(templateButton, templateButton.transform.position, templateButton.transform.rotation);
                    b.transform.SetParent(panel.transform, false);
                    b.transform.position = new Vector3(templateButton.transform.position.x, templateButton.transform.position.y - currOffset);
                    currOffset += offset;

                    buttons.Add(b); 
                }
                for (int i = 0; i < numOptions; i++)
                {
                    Chat.Answer answer = graph.current.answers[i];
                    Button currButton = buttons[i];//.Dequeue();
                    currButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer.text; //op;
                    currButton.onClick.AddListener(delegate { gotoNext(graph.current.answers.IndexOf(answer)); });
                    Color c = currButton.targetGraphic.color; 
                    currButton.targetGraphic.color = new Color(c.r, c.g, c.b, 0);
                }
            }
            else
            {
                if (currB == null)
                    currB = buttons[0];

                Graphic img = currB.targetGraphic;
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f*pauseCount/pauseMax);
                pauseCount++;
                if (pauseCount == fadeMax)
                {
                    resetCounts(); 
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                    int index = buttons.IndexOf(currB); 
                    if (index == buttons.Count - 1)
                    {
                        tState = TextState.done;
                    }
                    else
                    {
                        currB = buttons[index + 1];
                    }
                }
            }
        }
    }

    private void forceOptions()
    {
        destroyButtons(); 
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

                buttons.Add(b);
            }
            for (int i = 0; i < numOptions; i++)
            {
                Chat.Answer answer = graph.current.answers[i];
                Button currButton = buttons[i];//.Dequeue();
                currButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer.text; //op;
                currButton.onClick.AddListener(delegate { gotoNext(graph.current.answers.IndexOf(answer)); });
            }
        }
    }

}








