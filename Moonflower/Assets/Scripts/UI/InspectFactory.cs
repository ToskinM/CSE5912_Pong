using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectFactory
{
    public Dictionary<Sprite, string> GetName = new Dictionary<Sprite, string>(); 
    public InspectFactory()
    {
        IconFactory fac = new IconFactory(); 
        GetName.Add(fac.GetIcon(Constants.MOUSE_ICON), Constants.MOUSE_NAME);
        GetName.Add(fac.GetIcon(Constants.CATBAT_ICON), Constants.CATBAT_NAME);
        GetName.Add(fac.GetIcon(Constants.ORBY_ICON), Constants.ORBY_NAME);
        GetName.Add(fac.GetIcon(Constants.ORBYJR_ICON), Constants.ORBYJR_NAME);
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
        return "They/Them\nRambunctious Rodent\n\nEasily frightened and easily quelled, they'd love a quick nibble before they are felled.";
    }

    public string GetCatbat()
    {
        return "They/Them\nFlying Kitty\n\nTheir diving and dodging can cause you a fright, but they only seek water bait to bite.";
    }

    public string GetOrby()
    {
        return "They/Them\nFireball\n\nDefinitely volatile, they might be unstable. Be cautious and aware they portend a fable.";
    }

    public string GetOrbyJr()
    {
        return "They/Them\nBaby Fireball\n\nThey tend to be curious and love to befriend, though parents flare out these buds don't offend.";
    }
}
