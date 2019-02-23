using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLookup
{
    public const string MOONFLOWER_NAME = "Moon Flower";
    public const string WOLFAPPLE_NAME = "Wolf Apple";
    public const string HONEY_NAME = "Honey";
    public const string ROPE_NAME = "Rope";
    public const string JAR_NAME = "Jar";
    public const string PUMPKIN_NAME = "Pumpkin";
    public const string CHIPA_NAME = "Chipa";
    public const string FLUTE_NAME = "Flute";

    public GameObject GetObject(string name)
    {
        switch (name)
        {
            case MOONFLOWER_NAME:
            //return Instantiate(); 
            default:
                return null;
        }
    }

    public Sprite GetSprite(string name)
    {
        switch (name)
        {
            case MOONFLOWER_NAME:
                return Resources.Load<Sprite>(Constants.MOONFLOWER_ICON);
            case WOLFAPPLE_NAME:
                return Resources.Load<Sprite>(Constants.WOLFAPPLE_ICON);
            case HONEY_NAME:
                return Resources.Load<Sprite>(Constants.HONEY_ICON);
            case ROPE_NAME:
                return Resources.Load<Sprite>(Constants.ROPE_ICON);
            case JAR_NAME:
                return Resources.Load<Sprite>(Constants.JAR_ICON);
            case PUMPKIN_NAME:
                return Resources.Load<Sprite>(Constants.PUMPKIN_ICON);
            case CHIPA_NAME:
                return Resources.Load<Sprite>(Constants.CHIPA_ICON);
            case FLUTE_NAME:
                return Resources.Load<Sprite>(Constants.FLUTE_ICON);
            default:
                return null;
        }
    }

    public string GetGuaraniName(string name)
    {
        switch (name)
        {
            case MOONFLOWER_NAME:
                return "jasy yvoty";
            case WOLFAPPLE_NAME:
                return "aguara yva";
            case HONEY_NAME:
                return "eirete";
            case ROPE_NAME:
                return "sa";
            case JAR_NAME:
                return "";
            case PUMPKIN_NAME:
                return "andai";
            case CHIPA_NAME:
                return "chipa";
            case FLUTE_NAME:
                return "mimby puku";
            default:
                return "";
        }
    }
}
