using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLookup
{

    public bool IsFood(string name)
    {
        switch (name)
        {
            case Constants.HONEY_NAME:
            case Constants.PUMPKIN_NAME:
            case Constants.CHIPA_NAME:
            case Constants.CORN_NAME:
            case Constants.SWEETPOTATO_NAME:
            case Constants.PINEAPPLE_NAME:
            case Constants.FISH_NAME:
            case Constants.PEANUT_NAME:
            case Constants.MUSHROOM_NAME:
                return true;
            
            default:
                return false;
        }
    }

    public bool IsSweet(string name)
    {
        switch (name)
        {
            case Constants.HONEY_NAME:
            case Constants.PINEAPPLE_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsInstrument(string name)
    {
        switch (name)
        {
            case Constants.FLUTE_NAME:
            case Constants.DRUM_NAME:
                return true;
            default:
                return false;
        }
    }

    public bool IsMaterial(string name)
    {
        switch (name)
        {
            case Constants.ROPE_NAME:
            case Constants.PAINT_NAME:
            case Constants.PUMPKIN_NAME:
            case Constants.FEATHER_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsWeapon(string name)
    {
        switch (name)
        {
            case Constants.BOW_NAME:
            case Constants.ARROW_NAME:
            case Constants.STAFF_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsContainer(string name)
    {
        switch (name)
        {
            case Constants.JAR_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsDecorative(string name)
    {
        switch (name)
        {
            case Constants.NECKLACE_NAME:
            case Constants.PAINT_NAME:
                return true;

            default:
                return false;
        }
    }

    public bool IsLifeObject(string name)
    {
        return name.Equals(Constants.WOLFAPPLE_NAME) || name.Equals(Constants.MOONFLOWER_NAME); 
    }

    public GameObject GetObject(string name)
    {
        switch (name)
        {
            case Constants.MOONFLOWER_NAME:
            //return Instantiate(); 
            default:
                return null;
        }
    }

    public Sprite GetSprite(string name)
    {
        switch (name)
        {
            case Constants.MOONFLOWER_NAME:
                return Resources.Load<Sprite>(Constants.MOONFLOWER_ICON);
            case Constants.WOLFAPPLE_NAME:
                return Resources.Load<Sprite>(Constants.WOLFAPPLE_ICON);
            case Constants.HONEY_NAME:
                return Resources.Load<Sprite>(Constants.HONEY_ICON);
            case Constants.ROPE_NAME:
                return Resources.Load<Sprite>(Constants.ROPE_ICON);
            case Constants.JAR_NAME:
                return Resources.Load<Sprite>(Constants.JAR_ICON);
            case Constants.PUMPKIN_NAME:
                return Resources.Load<Sprite>(Constants.PUMPKIN_ICON);
            case Constants.CHIPA_NAME:
                return Resources.Load<Sprite>(Constants.CHIPA_ICON);
            case Constants.FLUTE_NAME:
                return Resources.Load<Sprite>(Constants.FLUTE_ICON);
            case Constants.PINEAPPLE_NAME:
                return Resources.Load<Sprite>(Constants.PINEAPPLE_ICON);
            case Constants.FISH_NAME:
                return Resources.Load<Sprite>(Constants.FISH_ICON);
            case Constants.PEANUT_NAME:
                return Resources.Load<Sprite>(Constants.PEANUT_ICON);
            case Constants.BOW_NAME:
                return Resources.Load<Sprite>(Constants.BOW_ICON);
            case Constants.ARROW_NAME:
                return Resources.Load<Sprite>(Constants.ARROW_ICON);
            case Constants.CORN_NAME:
                return Resources.Load<Sprite>(Constants.CORN_ICON);
            case Constants.STAFF_NAME:
                return Resources.Load<Sprite>(Constants.STAFF_ICON);
            case Constants.PAINT_NAME:
                return Resources.Load<Sprite>(Constants.PAINT_ICON);
            case Constants.SWEETPOTATO_NAME:
                return Resources.Load<Sprite>(Constants.SWEETPOTATO_ICON);
            case Constants.DRUM_NAME:
                return Resources.Load<Sprite>(Constants.DRUM_ICON);
            case Constants.FEATHER_NAME:
                return Resources.Load<Sprite>(Constants.FEATHER_ICON);
            case Constants.NECKLACE_NAME:
                return Resources.Load<Sprite>(Constants.NECKLACE_ICON);
            case Constants.MUSHROOM_NAME:
                return Resources.Load<Sprite>(Constants.MUSHROOM_ICON);
            default:
                return null;
        }
    }

    public string GetGuaraniName(string name)
    {
        switch (name)
        {
            case Constants.MOONFLOWER_NAME:
                return "jasy yvoty";
            case Constants.WOLFAPPLE_NAME:
                return "aguara yva";
            case Constants.HONEY_NAME:
                return "eirete";
            case Constants.ROPE_NAME:
                return "sa";
            case Constants.JAR_NAME:
                return "japepo";
            case Constants.PUMPKIN_NAME:
                return "andai";
            case Constants.CHIPA_NAME:
                return "chipa";
            case Constants.FLUTE_NAME:
                return "mimby puku";
            case Constants.PINEAPPLE_NAME:
                return "anana";
            case Constants.SWEETPOTATO_NAME:
                return "jety";
            case Constants.PEANUT_NAME:
                return "manduvi";
            case Constants.FISH_NAME:
                return "pira";
            case Constants.PAINT_NAME:
                return "mbosay";
            case Constants.STAFF_NAME:
                return "pokoka";
            case Constants.CORN_NAME:
                return "avati";
            case Constants.BOW_NAME:
                return "yvyrapa";
            case Constants.ARROW_NAME:
                return "hu'y";
            case Constants.FEATHER_NAME:
                return "ague";
            case Constants.DRUM_NAME:
                return "mbotapu";
            case Constants.NECKLACE_NAME:
                return "jeguaka";
            case Constants.MUSHROOM_NAME:
                return "urupero";
            default:
                return "";
        }
    }
}
