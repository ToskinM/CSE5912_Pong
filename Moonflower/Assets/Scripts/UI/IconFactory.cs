using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFactory
{

    public Sprite GetIcon(string file)
    {
        return Resources.Load<Sprite>(file);
    }

    public Sprite GetIconFromName(string name)
    {
        switch (name)
        {
            case Constants.AMARU_NAME:
                return Resources.Load<Sprite>(Constants.AMARU_ICON);
            case Constants.NAIA_NAME:
                return Resources.Load<Sprite>(Constants.NAIA_ICON);
            case Constants.PINON_NAME:
                return Resources.Load<Sprite>(Constants.PINON_ICON);
            case Constants.SYPAVE_NAME:
                return Resources.Load<Sprite>(Constants.SYPAVE_ICON);
            case Constants.JERUTI_NAME:
                return Resources.Load<Sprite>(Constants.JERUTI_ICON);
            //case Constants.YSAPY_NAME:
            //return Resources.Load<Sprite>(Constants.YSAPY_ICON);
            case Constants.MOUSE_NAME:
                return Resources.Load<Sprite>(Constants.MOUSE_ICON);
            case Constants.CATBAT_NAME:
                return Resources.Load<Sprite>(Constants.CATBAT_ICON);
            case Constants.BRIMP_NAME:
                return Resources.Load<Sprite>(Constants.BRIMP_ICON);
            case Constants.ORBY_NAME:
                return Resources.Load<Sprite>(Constants.ORBY_ICON);
            case Constants.ORBYJR_NAME:
                return Resources.Load<Sprite>(Constants.ORBYJR_ICON);
            case Constants.TEJU_NAME:
                return Resources.Load<Sprite>(Constants.TEJU_ICON);
            default:
                return null;
        }
    }
}
