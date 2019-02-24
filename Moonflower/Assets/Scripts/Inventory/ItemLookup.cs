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
    public const string PINEAPPLE_NAME = "Pineapple";
    public const string SWEETPOTATO_NAME = "Sweet Potato";
    public const string PEANUT_NAME = "Peanut";
    public const string FISH_NAME = "Fish";
    public const string PAINT_NAME = "Paint";
    public const string STAFF_NAME = "Staff";
    public const string CORN_NAME = "Corn";


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
            case PINEAPPLE_NAME:
                return "anana";
            case SWEETPOTATO_NAME:
                return "jety";
            case PEANUT_NAME:
                return "manduvi";
            case FISH_NAME:
                return "pira";
            case PAINT_NAME:
                return "mbosay";
            case STAFF_NAME:
                return "pokoka";
            case CORN_NAME:
                return "avati"; 
            default:
                return "";
        }
    }
}
