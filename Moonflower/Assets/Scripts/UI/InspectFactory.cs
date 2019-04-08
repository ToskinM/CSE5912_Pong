using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectFactory
{
    public Dictionary<Sprite, string> GetName = new Dictionary<Sprite, string>();

    string pronouns=""; 

    public InspectFactory()
    {
        float rand = Random.Range(0f, 1f);
        if (rand > 0.3f && rand < 0.35f)
        {
            pronouns = "She/They";
        }
        else if (rand > 0.7f && rand < 0.75f)
        {
            pronouns = "He/They";
        }
        else if (rand > 0.125f && rand < 0.15f)
        {
            pronouns = "Ze/Zir";
        }
        else if(rand > 0.6f)
        {
            pronouns = "She/Her";
        }
        else if (rand > 0.2f)
        {
            pronouns = "He/Him";
        }
        else
        {
            pronouns = "They/Them";
        }

        IconFactory fac = new IconFactory(); 
        GetName.Add(fac.GetIcon(Constants.MOUSE_ICON), Constants.MOUSE_NAME);
        GetName.Add(fac.GetIcon(Constants.CATBAT_ICON), Constants.CATBAT_NAME);
        GetName.Add(fac.GetIcon(Constants.ORBY_ICON), Constants.ORBY_NAME);
        GetName.Add(fac.GetIcon(Constants.ORBYJR_ICON), Constants.ORBYJR_NAME);
    }

    public string Get(string name)
    {
        switch(name)
        {
            case Constants.NAIA_NAME:
                return "She/Her\nScavenger\n\nBelieve her bark is as bad as her bite, and she is always ready to fight.";
            case Constants.AMARU_NAME:
                return "He/Him\nGrower\n\nThis boy cannot possibly contain his love for his neighbors, his food, or his grain.";
            case Constants.PINON_NAME:
                return "He/Him\nChild\n\nEager to sneak, this little brother gets you in trouble with your mother.";
            case Constants.SYPAVE_NAME:
                return "She/They\nShaman\n\nHealer and leader to all around, personal attention does not abound.";
            case Constants.JERUTI_NAME:
                return "She/Her\nCrafter\n\nHer heart may be bitter with grief, but her hands still long for creative relief.";
            case Constants.YSAPY_NAME:
                return "He/Him\nScavenger\n\nAlways finding a way to get away, maybe he'll say why he ventures one day.";
            case Constants.MOUSE_NAME:
                return pronouns + "\nRambunctious Rodent\n\nEasily frightened and easily quelled, they'd love a quick nibble before they are felled.";
            case Constants.ANGUJA_NAME:
                return "They/Them\nRambunctious Rodent\n\nEars are large and ready to hear, they would never be insincere.";
            case Constants.IJAPUVA_NAME:
                return "He/Him\nRambunctious Rodent\n\nTelling tales much taller than he, be warned of what he claims to see.";
            case Constants.CATBAT_NAME:
                return pronouns + "\nFlying Kitty\n\nTheir diving and dodging can cause you a fright, but they only seek water bait to bite.";
            case Constants.ORBY_NAME:
                return pronouns + "\nFireball\n\nDefinitely volatile, they might be unstable. Be cautious and aware they portend a fable.";
            case Constants.ORBYJR_NAME:
                return pronouns + "\nBaby Fireball\n\nThey tend to be curious and love to befriend, though parents flare out these buds don't offend.";
            default:
                return "";

        }
    }

    public string GetNaia()
    {
        return "She/Her\nScavenger\n\nBelieve her bark is as bad as her bite, and she is always ready to fight.";
    }

    public string GetAmaru()
    {
        return "He/Him\nGrower\n\nThis boy cannot possibly contain his love for his neighbors, his food, or his grain.";
    }

    public string GetPinon()
    {
        return "He/Him\nChild\n\nEager to sneak, this little brother gets you in trouble with your mother.";
    }

    public string GetSypave()
    {
        return "She/They\nShaman\n\nHealer and leader to all around, personal attention does not abound.";
    }

    public string GetJeruti()
    {
        return "She/Her\nCrafter\n\nHer heart may be bitter with grief, but her hands still long for creative relief.";
    }

    public string GetYsapy()
    {
        return "He/Him\nScavenger\n\nAlways finding a way to get away, maybe he'll say why he ventures one day.";
    }

    public string GetMouse()
    {
        return pronouns + "\nRambunctious Rodent\n\nEasily frightened and easily quelled, they'd love a quick nibble before they are felled.";
    }

    public string GetCatbat()
    {
        return pronouns + "\nFlying Kitty\n\nTheir diving and dodging can cause you a fright, but they only seek water bait to bite.";
    }

    public string GetOrby()
    {
        return pronouns + "\nFireball\n\nDefinitely volatile, they might be unstable. Be cautious and aware they portend a fable.";
    }

    public string GetOrbyJr()
    {
        return pronouns + "\nBaby Fireball\n\nThey tend to be curious and love to befriend, though parents flare out these buds don't offend.";
    }
}
