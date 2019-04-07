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
    RectTransform panelTransform;
    DialoguePanelInfo panelInfo;

    List<Button> buttons;
    Sprite icon;
    string spriteFile;
    string exitText = "You have to go? Okay, see you around!";


    enum PanelState { down, rising, up, falling }
    PanelState pState = PanelState.down;
    enum TextState { typing, paused, options, done, ending }
    TextState tState = TextState.done;
    int typeIndex = 0;
    const int fadeMax = 30;
    Button currB;


    int slowDownFrac = 2;
    int pauseCount = 0;
    int pauseMax = 10;
    string punctuation = ".!?";

    DialogueGraph graph;
    DialogueFactory factory;
    GameObject partner;
    InteractionPopup interaction;

    string gName;

    public DialogueTrigger(GameObject person, GameObject p, Sprite iconSprite, string graphName)
    {
        partner = person; 
        panel = p;
        panelTransform = panel.GetComponent<RectTransform>();
        panelInfo = panel.GetComponent<DialoguePanelInfo>();
        buttons = new List<Button>();

        factory = new DialogueFactory();
        graph = factory.GetDialogue(graphName);
        graph.Restart();
        gName = graphName;

        icon = iconSprite;
        interaction = GameObject.Find("Interaction Popup").GetComponent<InteractionPopup>();
        //spriteFile = characterSprite;

    }

    public void Update()
    {
        //disable if panel down and enable is panel is up 
     //   Debug.Log(gName + " updating");
        switch (pState)
        {
            case PanelState.rising:
                panelInfo.IsUp = true;
                panel.transform.position += new Vector3(0, 4, 0);
                if (panel.transform.position.y >= panelInfo.UpPosition.y)
                {
                    panel.transform.position = panelInfo.UpPosition;
                    pState = PanelState.up;
                }
                break;

            case PanelState.falling:
                //                Debug.Log("Falling"); 
                panelInfo.Text.text = "";
                panelInfo.IsUp = true;
                panel.transform.position -= new Vector3(0, 4, 0);
                if (panel.transform.position.y <= panelInfo.DownPosition.y)
                {
                    pState = PanelState.down;
                    panelInfo.IsUp = false;
                }
                break;

            case PanelState.up:
                panelInfo.IsUp = true;
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
                    switch (tState)
                    {
                        case TextState.ending:
                            //display all the exit text
                            if (!panelInfo.Text.text.Equals(exitText))
                            {
                                panelInfo.Text.text = exitText;
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
                            //display all the dialogue text
                            panelInfo.Text.text = graph.current.text;
                            typeIndex = 0;
                            tState = TextState.options;
                            break;
                        case TextState.options:
                            //display all the available options
                            forceOptions();
                            tState = TextState.done;
                            break;
                        case TextState.done:
                            if (!hasOptions()) // if there are no option buttons
                            {
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

        }
        if (panelInfo.IsUp)
        {
            if (!panel.activeSelf)
            {
                panel.SetActive(true);
            }
        }
        else
        {
            pState = PanelState.down; 
            if (panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }
    }

    public void StartDialogue(bool disregardCombat = false, bool instantCam = false)
    {
        interaction.NotAllowed = true; 
        if (!PlayerController.instance.ActivePlayerCombatControls.InCombat || disregardCombat)
        {
            pState = PanelState.rising;
            panelInfo.Icon.sprite = icon;  //new IconFactory().GetIcon(spriteFile);
            tState = TextState.typing;
//            Debug.Log("parter null: " + (partner == null)); 
            //PlayerController.instance.TalkingPartner = partner;
            //PlayerController.instance.DisableSwitching();

            // Start dialogue camera this this npc 
            LevelManager.current.RequestDialogueCamera(partner, instantCam);

            PlayerController.instance.ActivePlayerCombatControls.OnHit += CombatCancelDialogue;
        }
    }

    private void CombatCancelDialogue(GameObject aggressor)
    {
        EndDialogue();
    }

    public void EndDialogue()
    {
        interaction.NotAllowed = false; 
        
        destroyButtons();
        pState = PanelState.falling; 
        engaged = false;

        // Exit dialogue camera 
        LevelManager.current.ReturnDialogueCamera();

        PlayerController.instance.ActivePlayerCombatControls.OnHit -= CombatCancelDialogue;
    }

    public bool DialogueActive()
    {
        return pState != PanelState.down;
    }

    public void SetExitText(string s)
    {
        exitText = s;
    }

    public void SetSelf(GameObject ob)
    {
        partner = ob; 
    }

    private void endConvo()
    {
        if (tState != TextState.ending)
        {
            tState = TextState.ending;
            panelInfo.Text.text = "";
            typeIndex = 0;
            destroyButtons();
        }
        else
        {
            complete = true;
            panelInfo.Text.text = exitText;
            EndDialogue();
        }
    }

    private void typeEnding()
    {
        if (!panelInfo.Text.text.Equals(exitText))
        {
            int currDiaIndex = typeIndex / slowDownFrac;
            if (typeIndex % slowDownFrac == 0 && currDiaIndex < exitText.Length)
                panelInfo.Text.text += exitText[currDiaIndex];

            typeIndex++;
        }
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
                    panelInfo.Text.text += dialogue[currDiaIndex]; //.Substring(0, typeIndex / slowDownFrac);
                }
                typeIndex++;
                if (currDiaIndex >= dialogue.Length)
                {
                    if (hasOptions())
                        tState = TextState.options;
                    else
                        tState = TextState.done;
                }
                else if (currDiaIndex > 1 && punctuation.IndexOf(dialogue[currDiaIndex - 1]) != -1)
                {
                    tState = TextState.paused;
                }

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
            NPCDialogueEvents.instance.IncreasePlayerCharisma(true);
            //terminate conversation
            EndDialogue();
            complete = true;
        }
        //reset panel
        destroyButtons();
        resetCounts();
        panelInfo.Text.text = "";
    }

    private void displayOptions()
    {
        int numOptions = graph.current.answers.Count;
        Button template = panelInfo.TemplateButton;
        if (numOptions > 0)
        {
            if (buttons.Count != numOptions)
            {
                int currOffset = 0;
                int offset = Screen.height / 21;
                for (int i = 0; i < numOptions; i++)
                {
                    Button b = Instantiate(template, template.transform.position, template.transform.rotation);
                    b.transform.SetParent(panel.transform, false);
                    b.transform.position = new Vector3(template.transform.position.x, template.transform.position.y - currOffset);
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

        Button template = panelInfo.TemplateButton;
        resetCounts();
        destroyButtons();
        int numOptions = graph.current.answers.Count;
        if (numOptions > 0)
        {
            int currOffset = 0;
            int offset = Screen.height / 21;
            for (int i = 0; i < numOptions; i++)
            {
                Button b = Instantiate(template, template.transform.position, template.transform.rotation);
                b.transform.SetParent(panel.transform, false);
                b.transform.position = new Vector3(template.transform.position.x, template.transform.position.y - currOffset);
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