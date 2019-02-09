using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    string prompt;
    List<string> options;
    Dictionary<string, Node> children;
    string defaultString = "default"; 

    public Node()
    {
        prompt = "";
        options = new List<string>();
        children = new Dictionary<string, Node>();
    }

    public Node(string text)
    {
        //Debug.Log("Create Node: " + text); 
        prompt = text;
        options = new List<string>();
        children = new Dictionary<string, Node>();
    }

    public Node(string text, List<string> optionNodes)
    {
        prompt = text;
        options = optionNodes;
        children = new Dictionary<string, Node>();
    }

    public string Prompt()
    {
        return prompt;
    }

    public bool HasOptions()
    {
        return options.Count > 0;
    }

    //public int NumChildren()
    //{
    //    return children.Count; 
    //}

    public List<string> Options()
    {
        return options;
    }

    public Node GetNext(string option)
    {
        if (children.ContainsKey(option))
            return children[option];
        else
        {
            Debug.Log("No node for: " + option); 
            return null;
        }
    }
    public Node GetNext()
    {
        if (children.ContainsKey(defaultString))
            return children[defaultString];
        else
        {
            Debug.Log("No default node for: " + prompt);
            return null;
        }
    }

    public void AddOption(string op)
    {
        options.Add(op);
    }

    public void AddResponse(string op, string resp)
    {
        if (!options.Contains(op))
            options.Add(op);

        Node n = new Node(resp);
        children.Add(op, n);
    }

    public void AddNext(string s)
    {
        Node n = new Node(s);
        children.Add(defaultString, n);
    }
}












