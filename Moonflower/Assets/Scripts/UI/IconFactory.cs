using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFactory
{

    public Sprite GetIcon(string file)
    {
        return Resources.Load<Sprite>(file);
    }
}
