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
        GetName.Add(fac.GetIcon(Constants.BRIMP_ICON), Constants.BRIMP_NAME);
        GetName.Add(fac.GetIcon(Constants.ORBYJR_ICON), Constants.ORBYJR_NAME);
    }

    public string Get(string name)
    {
        switch (name)
        {
            case Constants.NAIA_NAME:
                return "She/Her\nScavenger\n\nBelieve her bark is as bad as her bite, and she is always ready to fight.";
            case Constants.AMARU_NAME:
                return "He/Him\nGrower\n\nThis boy cannot possibly contain his love for his neighbors, his food, or his grain.";
            case Constants.PINON_NAME:
                return "He/Him\nChild\n\nEager to sneak, this little brother gets you in trouble with your mother.";
            case Constants.SYPAVE_NAME:
                return "She/They\nShaman\n\nHealer and leader to all around, motherly attention does not abound.";
            case Constants.JERUTI_NAME:
                return "She/Her\nCrafter\n\nHer heart may be bitter with grief, but her hands still long for creative relief.";
            case Constants.YSAPY_NAME:
                return "He/Him\nScavenger\n\nAlways finding a way to get away, maybe he'll say why he ventures one day.";
            case Constants.MOUSE_NAME:
                if (pronouns[0] == 'T' || pronouns[pronouns.IndexOf('/') + 1] == 'T')
                {
                    return pronouns + "\nRambunctious Rodent\n\nEasily frightened and easily quelled, they'd love a quick nibble before they are felled.";
                }
                else
                {
                    string sub = pronouns.Substring(0, pronouns.IndexOf('/'));
                    string pos = pronouns.Substring(pronouns.IndexOf('/') + 1);
                    return pronouns + "\nRambunctious Rodent\n\nEasily frightened and easily quelled, "+sub.ToLower()+"'d love a quick nibble before "+sub.ToLower()+" is felled.";
                }
            case Constants.ANGUJA_NAME:
                return "They/Them\nRambunctious Rodent\n\nEars are large and ready to hear, they would never be insincere.";
            case Constants.IJAPUVA_NAME:
                return "He/Him\nRambunctious Rodent\n\nTelling tales much taller than he, be warned of what he claims to see.";
            case Constants.EKIRIRI_NAME:
                return "Ze/Zir\nRambunctious Rodent\n\nDeaf to your words, ze can't comprehend why some noisy roommate would start to offend.";
            case Constants.CATBAT_NAME:
                if (pronouns[0] == 'T' || pronouns[pronouns.IndexOf('/') + 1] == 'T')
                {
                    return pronouns + "\nFlying Kitty\n\nThis nightcrawling creature can cause you a fright, but they only seek water bait to bite.";
                }
                else
                {
                    string sub = pronouns.Substring(0, pronouns.IndexOf('/'));
                    string pos = pronouns.Substring(pronouns.IndexOf('/') + 1);
                    return pronouns + "\nFlying Kitty\n\nThis nightcrawling creature can cause you a fright, but " + sub.ToLower() + " only seeks water bait to bite.";
                }
            case Constants.BRIMP_NAME:
                if (pronouns[0] == 'T' || pronouns[pronouns.IndexOf('/') + 1] == 'T')
                {
                    return pronouns + "\nBrimp\n\nTheir diving and dodging will give you a scare. It's best if you see them to wind up elsewhere.";
                }
                else
                {
                    string sub = pronouns.Substring(0, pronouns.IndexOf('/'));
                    string pos = pronouns.Substring(pronouns.IndexOf('/') + 1);
                    if (pos[0] == 'H')
                        return pronouns + "\nBrimp\n\nHis diving and dodging diving and dodging will give you a scare. It's best if you see " + sub.ToLower() + " to wind up elsewhere";
                    return pronouns + "\nBrimp\n\n" + pos + " diving and dodging diving and dodging will give you a scare. It's best if you see "+sub.ToLower()+" to wind up elsewhere";
                }
            case Constants.ORBY_NAME:
                if (pronouns[0] == 'T' || pronouns[pronouns.IndexOf('/') + 1] == 'T')
                {
                    return pronouns + "\nFireball\n\nDefinitely volatile, they might be unstable. Be cautious and aware they portend a fable.";
                }
                else
                {
                    string sub = pronouns.Substring(0, pronouns.IndexOf('/'));
                    string pos = pronouns.Substring(pronouns.IndexOf('/') + 1);
                    return pronouns + "\nFireball\n\nDefinitely volatile, "+sub.ToLower()+" might be unstable. Be cautious and aware "+sub.ToLower()+" portends a fable.";
                }
            case Constants.ORBYJR_NAME:
                if (pronouns[0] == 'T' || pronouns[pronouns.IndexOf('/') + 1] == 'T')
                {
                    return pronouns + "\nBaby Fireball\n\nThey tend to be curious and love to befriend, though parents flare out, this bud won't offend.";
                }
                else
                {
                    string sub = pronouns.Substring(0, pronouns.IndexOf('/'));
                    string pos = pronouns.Substring(pronouns.IndexOf('/') + 1);
                    return pronouns +"\nBaby Fireball\n\n"+sub+" tends to be curious and loves to befriend, though parents flare out, this bud won't offend.";
                }
            case Constants.TEJU_NAME:
                return "He/Him\nMonster God\n\nWhen left unchecked, the fires rain while yearning for sweet and receiving distain.";
            case Constants.TUVICHA_NAME:
                return "She/Her\nRambunctious Rodent\n\nLarge and loud this one does slumber, but she does not mean to encumber."; 
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

    public string GetTeju()
    {
        return "He/Him\nMonster God\n\nWhen left unchecked, the fires rain while yearning for sweet and receiving distain.";
    }
}
