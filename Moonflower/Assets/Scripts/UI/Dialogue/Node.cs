using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    string prompt;
    bool hasOptions;
    List<string> options;
    List<Node> children;

    public Node()
    {
        prompt = "";
        hasOptions = false;
        options = new List<string>();
        children = new List<Node>();
    }

    public Node(string text)
    {
        prompt = text;
        hasOptions = false;
        options = new List<string>();
        children = new List<Node>();
    }

    public Node(string text, List<string> optionNodes)
    {
        prompt = text;
        hasOptions = true;
        options = optionNodes;
        children = new List<Node>();
    }

    public string Prompt()
    {
        return prompt;
    }

    public bool HasOptions()
    {
        return hasOptions;
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
        return children[options.IndexOf(option)];
    }

    public void AddOption(string op)
    {
        options.Add(op);
        if (!hasOptions)
            hasOptions = true;
    }

    public void AddResponse(string op, string resp)
    {
        if (!options.Contains(op))
            options.Add(op);

        Node n = new Node(resp);
        children.Insert(options.IndexOf(op), n);
    }
    public void AddNext(string s)
    {
        Node n = new Node(s);
        children.Add(n);
    }
}












