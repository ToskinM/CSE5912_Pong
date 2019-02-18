using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XNode;
using Dialogue; 

public class AmaruDialogueTrigger : MonoBehaviour
{
    public bool Complete { get { return complete; } }
    bool complete = false;

    GameObject panel;
    Image icon; 
    TextMeshProUGUI text;
    Button templateButton; 
    Queue<Button> buttons;
    bool active = false;
    DialogueTreeNode dialogue;
    DialogueTreeNode currNode;
    ICommand freezeCommand;
    string spriteFile;
    bool optionsDisplayed = false;

    public DialogueGraph graph; 


    public AmaruDialogueTrigger(GameObject p, string characterSprite)
    {
        graph = Resources.Load<DialogueGraph>("New Dialogue Graph");
        graph.Restart();
        if(graph.current == null)
            Debug.Log("current is null"); 
        panel = p; //GameObject.Find("Dialogue Panel");
        icon = panel.transform.GetChild(0).GetComponent<Image>(); 
        text = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        buttons = new Queue<Button>();
        templateButton = panel.transform.GetChild(2).GetComponent<Button>();
        freezeCommand = new FreezeCameraCommand();
        spriteFile = characterSprite; 

        //dialogue = new DialogueTreeNode("Hi Anai!");
        //{
        //    string o, r; 
        //    o = "Hi, Amaru.";
        //    dialogue.AddOption(o);
        //    {
        //        r = "What are you up to on this beautiful day?";
        //        dialogue.AddResponse(o, r);
        //        DialogueTreeNode next1 = dialogue.GetNext(o); // gets node for r
        //        {
        //            o = "Just exploring.";
        //            r = "Well, I won't keep you then. Tell Mimbi I love her!";
        //            next1.AddResponse(o, r);
        //        }

        //        {
        //            o = "Admiring your beautiful garden!";
        //            r = "*blush* thank you! I'm so proud of these little guys.";
        //            next1.AddResponse(o, r);
        //            DialogueTreeNode next2 = next1.GetNext(o);
        //            next2.AddNext("Naia was looking for you, by the way.");
        //            next2 = next2.GetNext();

        //            {
        //                o = "Why?";
        //                r = "I think she was looking for a parter.";
        //                next2.AddResponse(o, r);
        //                DialogueTreeNode next3 = next2.GetNext(o);
        //                next3.AddNext("For sparring.");
        //                next3 = next3.GetNext(); 

        //                {
        //                    o = "Right.";
        //                    r = "You should give it a chance.";
        //                    next3.AddResponse(o, r);
        //                    DialogueTreeNode next4 = next3.GetNext(o);

        //                    next4.AddNext("It being sparring.");
        //                    next4 = next4.GetNext();
        //                    next4.AddNext("You should give sparring a chance.");
        //                    next4 = next4.GetNext();
        //                    next4.AddNext("With her.");
        //                    next4 = next4.GetNext();
        //                    next4.AddNext("You should give sparring a chance with her...");
        //                    next4 = next4.GetNext();
        //                    next4.AddNext("If you want.");
        //                    next4 = next4.GetNext();
        //                    next4.AddNext("No pressure.");
        //                    next4 = next4.GetNext();

        //                    {
        //                        o = "Yeah, I know...";
        //                        r = "Are you okay?";
        //                        next4.AddResponse(o, r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);

        //                        {
        //                            o = "Yeah, I'm fine!";
        //                            r = "Okay, just making sure.";
        //                            next5.AddResponse(o, r);
        //                            DialogueTreeNode next6 = next5.GetNext(o);
        //                            next6.AddNext("I support you no matter what.");
        //                        }

        //                        {
        //                            o = "I don't want to talk about it.";
        //                            r = "I understand.";
        //                            next5.AddResponse(o, r);
        //                            DialogueTreeNode next6 = next5.GetNext(o);
        //                            next6.AddNext("I'm here if you ever change your mind.");
        //                        }
        //                    }

        //                    {
        //                        o = "It's really none of your buisness.";
        //                        r = "*flush* I'm sorry, I just- I'm just trying-";
        //                        next4.AddResponse(o, r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);
        //                        next5.AddNext("I'm sorry.");
        //                    }

        //                    {
        //                        o = "I'll just go do that then.";
        //                        r = "Yeah? Good! Great. Sorry, I just...";
        //                        next4.AddResponse(o, r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);
        //                        next5.AddNext("*smile* Go have fun.");
        //                    }
        //                }

        //                {
        //                    o = "Oh, awesome!";
        //                    r = "Well, I won't keep you.";
        //                    next3.AddResponse(o, r);
        //                    DialogueTreeNode next4 = next3.GetNext(o);
        //                    next4.AddNext("Tell her I said hi!");
        //                }

        //                {
        //                    o = "I'm always up to be her partner...";
        //                    r = "Oh! Great! Well, she's waiting for you.";
        //                    next3.AddResponse(o, r);
        //                    DialogueTreeNode next4 = next3.GetNext(o);
        //                    next4.AddNext("She always is.");
        //                }
        //            }

        //            {
        //                o = "I'll go find her.";
        //                r = "Awesome! See you around!";
        //                next2.AddResponse(o, r);
        //            }

        //            {
        //                o = "She already found me.";
        //                r = "Oh! Awesome! How did- how did it go?";
        //                next2.AddResponse(o, r);
        //                DialogueTreeNode next3 = next2.GetNext(o);

        //                {
        //                    o = "What do you mean?";
        //                    r = "Oh, I mean- Um...";
        //                    next3.AddResponse(o, r);
        //                    DialogueTreeNode next4 = next3.GetNext(o);
        //                    next4.AddNext("Nevermind, I'll just...");
        //                    next4 = next4.GetNext();
        //                    next4.AddNext("Go...");
        //                }

        //                {
        //                    o = "It went great!";
        //                    r = "That's amazing! So she told you-";
        //                    next3.AddResponse(o, r);
        //                    DialogueTreeNode next4 = next3.GetNext(o);

        //                    {
        //                        o = "Told me what?";
        //                        r = "What? Oh, nothing. *nervous laugh*";
        //                        next4.AddResponse(o,r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);
        //                        next5.AddNext("I just need to- uh... talk to Naia real quick.");
        //                        next5 = next5.GetNext();
        //                        next5.AddNext("Bye!");
        //                    }

        //                    {
        //                        o = "We mostly just sparred";
        //                        next4.AddResponse(o, r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);
        //                        next5.AddNext("Oh, right of course.");
        //                        next5 = next5.GetNext();
        //                        next5.AddNext("Well, I'm glad you two had a chance to spar. I hope you'll find time to do it more in the future...?");
        //                        next5 = next5.GetNext();

        //                        {
        //                            o = "Oh yeah! Definitely.";
        //                            r = "That's good. I actually need to ask Naia something.";
        //                            next5.AddResponse(o, r);
        //                            DialogueTreeNode next6 = next5.GetNext(o);
        //                            next6.AddNext("See you around, Anai. And you too, Mimbi!");
        //                        }

        //                        {
        //                            o = "Doubt it. I don't like fighting.";
        //                            next5.AddResponse(o, r);
        //                            DialogueTreeNode next6 = next5.GetNext(o);
        //                            next6.AddNext("*laugh* Yeah, me neither. You can talk to Naia too, though.");
        //                            next6 = next6.GetNext();

        //                            {
        //                                o = "Yeah, I know.";
        //                                r = "That's good. I actually need to ask Naia something.";
        //                                next6.AddResponse(o, r);
        //                                DialogueTreeNode next7 = next6.GetNext(o);
        //                                next7.AddNext("See you around, Anai. And you too, Mimbi!");
        //                            }

        //                            {
        //                                o = "I've tried. She prefers fighting.";
        //                                r = "That she does.";
        //                                next6.AddResponse(o, r);
        //                                DialogueTreeNode next7 = next6.GetNext(o);
        //                                next7.AddNext("It can be hard. I hope you two figure it out.");

        //                            }

        //                            { 
        //                                o = "I don't feel the need.";
        //                                r = "Oh. Right.";
        //                                next6.AddResponse(o, r);
        //                                DialogueTreeNode next7 = next6.GetNext(o);
        //                                next7.AddNext("...");

        //                            }

        //                        }
        //                    }
        //                }

        //                {
        //                    o = "Definitely not ideal...";
        //                    r = "Oh, I'm sorry. I thought-";
        //                    next3.AddResponse(o, r);
        //                    DialogueTreeNode next4 = next3.GetNext(o);

        //                    {
        //                        o = "Thought what?";
        //                        r = "I- I thought- ";
        //                        next4.AddResponse(o, r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);
        //                        next5.AddNext("Nevermind, I'll just...");
        //                        next5 = next5.GetNext();
        //                        next5.AddNext("Go...");
        //                    }

        //                    {
        //                        o = "Whatever, it doesn't matter.";
        //                        r = "If you say so...";
        //                        next4.AddResponse(o, r);
        //                        DialogueTreeNode next5 = next4.GetNext(o);
        //                        next5.AddNext("Don't forget there are moonflowers over by your Mom's hut.");
        //                        next5 = next5.GetNext();
        //                        next5.AddNext("And wolfapples for Mimbi.");
        //                        next5 = next5.GetNext();
        //                        next5.AddNext("I'm gonna go check on Naia.");
        //                        next5 = next5.GetNext();
        //                    }
        //                }
        //            }
        //        }


        //        {
        //            o = "Looking for a fight...";
        //            r = "*laugh* Well I'm not the one you'll want for that.";
        //            next1.AddResponse(o, r);
        //            DialogueTreeNode next2 = next1.GetNext(o);
        //            next2.AddNext("Naia's always ready for a sparring match though.");
        //            next2 = next2.GetNext();
        //            next2.AddNext("I'd suggest going to find her.");
        //        }

        //    }

        //    {
        //        o = "Can't talk right now";
        //        dialogue.AddResponse(o, "Oh, okay. See you around then!");
        //    }
        //}

        currNode = dialogue;
    }


    public void Update()
    {
        if (active)
        {
            //text.text = currNode.Prompt();
            text.text = graph.current.text; 
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if (!optionsDisplayed)
                {
                    displayOptions();
                    optionsDisplayed = true;
                }
                else
                {
                    //continueToNextNode();
                    gotoNext(-1); 
                    optionsDisplayed = false;
                }
            }
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
    private void gotoNext(int i)
    {
        graph.current = graph.GetNext(i);
        Debug.Log("did we get next?");
        Debug.Log("Next should be: " + graph.current.text); 
        //currNode = currNode.GetNext(s);
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
        //if (currNode.HasOptions())
        int numOptions = graph.current.answers.Count; 
        if(numOptions > 0)
        {
            int currOffset = 0;
            int offset = Screen.height / 17;
            //for (int i = 0; i < currNode.Options().Count; i++)
            for (int i = 0; i < numOptions; i++)
            {
                Button b = Instantiate(templateButton, templateButton.transform.position, templateButton.transform.rotation);
                b.transform.SetParent(panel.transform, false);
                b.transform.position = new Vector3(templateButton.transform.position.x, templateButton.transform.position.y - currOffset);
                currOffset += offset;

                buttons.Enqueue(b);
            }
            //foreach (string op in currNode.Options())
            for(int i  = 0;  i < numOptions;  i++)
            {
                Chat.Answer answer = graph.current.answers[i];
                Debug.Log("so " + i + " is " + answer.text);
                Button currButton = buttons.Dequeue();
                currButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer.text; //op;
                //currButton.onClick.AddListener(delegate { changeCurrNode(op); });
                currButton.onClick.AddListener(delegate { gotoNext(graph.current.answers.IndexOf(answer)); });
                buttons.Enqueue(currButton);
            }
        }
    }

}








