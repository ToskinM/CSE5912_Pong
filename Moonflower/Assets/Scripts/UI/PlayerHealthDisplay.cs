using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthDisplay : MonoBehaviour
{
    public GameObject Flower;
    public GameObject Apple; 
    public GameObject Player;

    public bool Dead = false;

    private bool playerIsAnai = true; 

    private LifeAppleController appleControl;
    private LifeFlowerController flowerControl; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Is this not happening?"); 
        appleControl = new LifeAppleController(Apple);
        flowerControl = new LifeFlowerController(Flower);

    }

    public void Update()
    {
        if (playerIsAnai)
            flowerControl.UpdateFlower();
        else
            appleControl.UpdateApple(); 
    }

    public void HitHealth()
    {
        if (playerIsAnai)
            flowerControl.HitHealth();
        else
            appleControl.HitHealth(); 

    }

    public void HealPetal()
    {
        if (playerIsAnai)
            flowerControl.HealPetal();
        else
            appleControl.HealApple(); 
    }

    public void SwitchPlayer()
    {
        playerIsAnai = !playerIsAnai;
        if(playerIsAnai)
        {
            Flower.SetActive(true);
            Apple.SetActive(false); 
        }
        else
        {
            Flower.SetActive(false);
            Apple.SetActive(true);
        }
    }

}
