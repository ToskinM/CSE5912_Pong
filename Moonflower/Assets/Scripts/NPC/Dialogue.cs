using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public List<DialogueNode> Nodes;

    public void AddNode(DialogueNode node)
    {
        Nodes.Add(node);
        node.NodeID = Nodes.IndexOf(node); 
    }

    public void AddOption(string response, DialogueNode curr, DialogueNode dest)
    {
        if(!Nodes.Contains(dest))
        {
            AddNode(dest);
        }

        if(!Nodes.Contains(curr))
        {
            AddNode(curr);
        }

        DialogueOption opt = new DialogueOption(response, dest.NodeID);
        curr.Options.Add(opt); 
    }

    public Dialogue()
    {

    }

}
