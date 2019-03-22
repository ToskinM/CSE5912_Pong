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
    public const string SWEETPOTATO_NAME = "Sweet potato";
    public const string PEANUT_NAME = "Peanut";
    public const string FISH_NAME = "Fish";
    public const string PAINT_NAME = "Paint";
    public const string STAFF_NAME = "Staff";
    public const string CORN_NAME = "Corn";
    public const string BOW_NAME = "Bow";
    public const string ARROW_NAME = "Arrow";
    public const string FEATHER_NAME = "Feather";
    public const string DRUM_NAME = "Drum";
    public const string NECKLACE_NAME = "Necklace";


    public bool IsFood(string name)
    {
        switch (name)
        {
            case HONEY_NAME:
            case PUMPKIN_NAME:
            case CHIPA_NAME:
            case CORN_NAME:
            case SWEETPOTATO_NAME:
            case PINEAPPLE_NAME:
            case FISH_NAME:
            case PEANUT_NAME:
                return true;
            
            default:
                return false;
        }
    }

    public bool IsInstrument(string name)
    {
        switch (name)
        {
            case FLUTE_NAME:
            case DRUM_NAME:
                return true;
            default:
                return false;
        }
    }

    public bool IsMaterial(string name)
    {
        switch (name)
        {
            case ROPE_NAME:
            case PAINT_NAME:
            case PUMPKIN_NAME:
            case FEATHER_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsWeapon(string name)
    {
        switch (name)
        {
            case BOW_NAME:
            case ARROW_NAME:
            case STAFF_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsContainer(string name)
    {
        switch (name)
        {
            case JAR_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsLifeObject(string name)
    {
        return name.Equals(WOLFAPPLE_NAME) || name.Equals(MOONFLOWER_NAME); 
    }

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
            case PINEAPPLE_NAME:
                return Resources.Load<Sprite>(Constants.PINEAPPLE_ICON);
            case FISH_NAME:
                return Resources.Load<Sprite>(Constants.FISH_ICON);
            case PEANUT_NAME:
                return Resources.Load<Sprite>(Constants.PEANUT_ICON);
            case BOW_NAME:
                return Resources.Load<Sprite>(Constants.BOW_ICON);
            case ARROW_NAME:
                return Resources.Load<Sprite>(Constants.ARROW_ICON);
            case CORN_NAME:
                return Resources.Load<Sprite>(Constants.CORN_ICON);
            case STAFF_NAME:
                return Resources.Load<Sprite>(Constants.STAFF_ICON);
            case PAINT_NAME:
                return Resources.Load<Sprite>(Constants.PAINT_ICON);
            case SWEETPOTATO_NAME:
                return Resources.Load<Sprite>(Constants.SWEETPOTATO_ICON);
            case DRUM_NAME:
                return Resources.Load<Sprite>(Constants.DRUM_ICON);
            case FEATHER_NAME:
                return Resources.Load<Sprite>(Constants.FEATHER_ICON);
            case NECKLACE_NAME:
                return Resources.Load<Sprite>(Constants.NECKLACE_ICON);
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
                return "japepo";
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
            case BOW_NAME:
                return "yvyrapa";
            case ARROW_NAME:
                return "hu'y";
            case FEATHER_NAME:
                return "ague";
            case DRUM_NAME:
                return "mbotapu";
            case NECKLACE_NAME:
                return "jeguaka";
            default:
                return "";
        }
    }
}
