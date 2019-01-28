using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro; 

public class DialogueTree
{
    GameObject textPanel;
    TextMeshPro text;  

    public bool DialogueFinished { get { return dialogueFinished; } }

    bool dialogueFinished = false; 
    DialogueNode dTreeRoot;
    Dictionary<DialogueNode, List<DialogueNode>> dTree;
    string promptTag = "Prompt";
    string optTag = "Option";
    string typeName = "type";
    string wordsname = "words"; 

    public DialogueTree(string xmlfile)
    {
        textPanel = GameObject.Find("Dialogue Panel");
        text = textPanel.transform.GetChild(0).GetComponent<TextMeshPro> (); 
        dTree = new Dictionary<DialogueNode, List<DialogueNode>>(); 

        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(xmlfile);
        XmlNode xmlRoot = xmldoc;
        xmlRoot = xmlRoot.FirstChild; //get past xml initilization 
        dTreeRoot = makeDialogueNode(xmlRoot);
        processXml(xmlRoot, dTreeRoot); 
    }

    public DialogueNode StartDialogue()
    {
        textPanel.SetActive(true); 
        return dTreeRoot; 
    }

    public void EndDialogue()
    {
        textPanel.SetActive(false);
        text.text = ""; 
        dialogueFinished = true; 
    }

    public List<DialogueNode> GetOptions(DialogueNode n)
    {
        text.text = "";
        List<DialogueNode> options = new List<DialogueNode>();
        int numOptions = dTree[n].Count; 
        //Vector3 topPos = anchoredPosition
        foreach (DialogueNode child in dTree[n])
        {
            if(child.IsOption)
            {
                options.Add(child.GetCopy()); 
            }
        }
        dTree.Remove(n); //remove prompt
        return options; 
    }

    public DialogueNode GetResponse(DialogueNode n)
    {
        DialogueNode response = null; 
        if(dTree[n].Count > 0)
        {
            response = dTree[n][0]; 
        }
        dTree.Remove(n); //remove option
        
        return response; 
    }

    private DialogueNode makeDialogueNode(XmlNode node)
    {
        DialogueNode dTreeNode = null;
        if (node.Attributes[typeName].Value.Equals(promptTag))
            dTreeNode = new DialoguePrompt(node.Attributes[wordsname].Value);
        else if (node.Attributes[typeName].Value.Equals(optTag))
            dTreeNode = new DialogueOption(node.Attributes[wordsname].Value);
                    

        return dTreeNode;
    }

    //turn xml into dialogue tree
    private void processXml(XmlNode xml, DialogueNode dTreeRootNode)
    {
        //make list for the dialogue tree children
        List<DialogueNode> dTreeChildren = new List<DialogueNode>(); 

        //recurse if xml root has children
        if(xml.HasChildNodes)
        {
            //process every child of xml root
            foreach (XmlNode xmlNode in xml.ChildNodes)
            {
                //see which type of node the xml node is
                DialogueNode dTreeChildNode = makeDialogueNode(xmlNode);

                //add child dialogue node to the list of children of the root dialogue tree node
                if (dTreeChildNode != null)
                {
                    dTreeChildren.Add(dTreeChildNode);
                    processXml(xmlNode, dTreeChildNode); //recurse of child
                }
            }

        }
        //add dialogue tree root and its children to the dialogue tree 
        dTree.Add(dTreeRootNode, dTreeChildren); 

    }


}
