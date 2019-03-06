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
    public bool engaged = false;

    GameObject panel;
    Image icon;
    TextMeshProUGUI text;
    Button templateButton;
    //ICommand freezeCommand;

    List<Button> buttons;
    string spriteFile;
    string exitText = "You have to go? Okay, see you around!";


    enum PanelState { down, rising, up, falling }
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
    bool noExit = true; 


    public DialogueTrigger(GameObject p, string characterSprite, string graphName)
    {
        panel = p;
        icon = panel.transform.GetChild(0).GetComponent<Image>();
        text = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        buttons = new List<Button>();
        templateButton = panel.transform.GetChild(2).GetComponent<Button>();

        upPos = panel.transform.position;
        downPos = new Vector3(upPos.x, upPos.y - icon.rectTransform.rect.height);
        panel.transform.position = downPos;

        factory = new DialogueFactory();
        graph = factory.GetDialogue(graphName);
        graph.Restart();

        //freezeCommand = new FreezeCameraCommand();
        spriteFile = characterSprite;

    }

    public void Update()
    {
        if (pState != PanelState.down)
        {
            switch (pState)
            {
                case PanelState.rising:
                    Debug.Log("Panel is rising");
                    panel.transform.position += new Vector3(0, 4, 0);
                    if (panel.transform.position.y >= upPos.y)
                    {
                        panel.transform.position = upPos;
                        pState = PanelState.up;
                    }
                    break;

                case PanelState.falling:
                    Debug.Log("Panel is falling");
                    panel.transform.position -= new Vector3(0, 4, 0);
                    if (panel.transform.position.y <= downPos.y)
                    {
                        pState = PanelState.down;
                    }
                    break;

                case PanelState.up:
                    Debug.Log("Panel is up"); 
                    //easy exit 
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        endConvo();
                    }

                    //type out dialgoue text 
                    switch (tState)
                    {
                        //type out the ending text all pretty
                        case TextState.ending:
                            typeEnding();
                            break;
                        //type out the dialogue text all pretty
                        case TextState.typing:
                        case TextState.paused:
                            typeText();
                            break;
                        //display the options all pretty
                        case TextState.options:
                            displayOptions();
                            break;
                    }

                    //easy skip through
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("We got a space bar!"); 
                        //if (!engaged)
                        //{
                        //    freezeCommand.Execute();
                        //    engaged = true;
                        //}
                        switch (tState)
                        {
                            case TextState.ending:
                                //display all the exit text
                                if(!text.text.Equals(exitText))
                                {
                                    text.text = exitText;
                                }
                                //deactivate dialogue convo
                                else
                                {
                                    //pState = PanelState.falling;
                                    complete = true; 
                                    EndDialogue();
                                }
                                break; 
                            case TextState.typing:
                            case TextState.paused:
                                Debug.Log("we gonna fill in the text");
                                //display all the dialogue text
                                text.text = graph.current.text;
                                typeIndex = 0;
                                tState = TextState.options;
                                break;
                            case TextState.options:
                                Debug.Log("we gonna fill in the options");
                                //display all the available options
                                forceOptions();
                                tState = TextState.done;
                                break;
                            case TextState.done:
                                Debug.Log("we gonna go to next option");
                                if (!hasOptions()) // if there are no option buttons
                                {
                                    Debug.Log("for real");
                                    //go to next node in tree (no branching)
                                    gotoNext(-1);
                                }
                                break;
                            default:
                                Debug.Log("This shouldn't happen");
                                break;
                        }
                    }
                    break;
                case PanelState.down:
                    panel.SetActive(false);
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
        //freezeCommand.Execute();

        // Start dialogue camera this this npc 
        LevelManager.current.RequestDialogueCamera();

        LevelManager.current.anai.OnHit += CombatCancelDialogue;
    }

    private void CombatCancelDialogue(GameObject aggressor)
    {
        EndDialogue();
    }

    public void EndDialogue()
    {
        //panel.SetActive(false);
        destroyButtons();
        //active = false;
        pState = PanelState.falling;
        Debug.Log("set panel falling"); 
        //freezeCommand.Unexecute();
        engaged = false;

        // Exit dialogue camera 
        LevelManager.current.ReturnDialogueCamera();
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
        if (tState != TextState.ending)
        {
            tState = TextState.ending;
            text.text = "";
            typeIndex = 0;
            destroyButtons();
        }
        else
        {
            complete = true;
            text.text = exitText;
            EndDialogue(); 
        }
    }

    private void typeEnding()
    {
        int currDiaIndex = typeIndex / slowDownFrac;
        if (typeIndex % slowDownFrac == 0 && currDiaIndex < exitText.Length)
            text.text += exitText[currDiaIndex];

        typeIndex++;
        //if (currDiaIndex == dialogue.Length)
        //{
        //    typeIndex = 0;
        //    typing = false;
        //    state = TextState.options;  
        //}
        //if (currDiaIndex > 1 && punctuation.IndexOf(exitText[currDiaIndex - 2]) != -1)
            //tState = TextState.paused;
    }

    private void typeText()
    {
        string dialogue = graph.current.text;
        int currDiaIndex = typeIndex / slowDownFrac;
        switch (tState)
        {
            case TextState.typing:

                if (typeIndex % slowDownFrac == 0 && currDiaIndex < dialogue.Length)
                {
                    text.text += dialogue[currDiaIndex]; //.Substring(0, typeIndex / slowDownFrac);
                }
                typeIndex++;
                if (currDiaIndex >= dialogue.Length)
                {
                    if (hasOptions())
                        tState = TextState.options;
                    else
                        tState = TextState.done;
                }
                else if (currDiaIndex > 1 && punctuation.IndexOf(dialogue[currDiaIndex - 2]) != -1)
                    tState = TextState.paused;

                //if (currDiaIndex >= dialogue.Length)
                //{
                //    typeIndex = 0; 
                //}

                //if (currDiaIndex == dialogue.Length)
                //{
                //    typeIndex = 0;
                //    typing = false;
                //    state = TextState.options;  
                //}

                break;

            case TextState.paused:
                pauseCount++;
                if (pauseCount == pauseMax)
                {
                    pauseCount = 0;
                    if (currDiaIndex >= dialogue.Length)
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
        while (buttons.Count > 0)
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
        if (!engaged)
        {
            //freezeCommand.Execute();
            engaged = true;
        }

        //set to type out new dialogue text 
        tState = TextState.typing;
        string prev = graph.current.text;
        graph.GetNext(i);
        string next = graph.current.text;

        //if we didn't go to a new node on the tree, then we're at end of branch
        if (next.Equals(prev))
        {
            //terminate conversation
            Debug.Log("we finished");
            EndDialogue();
            complete = true;
        }
        //reset panel
        destroyButtons();
        resetCounts();
        text.text = ""; 
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
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f * pauseCount / pauseMax);
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


        resetCounts();
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
