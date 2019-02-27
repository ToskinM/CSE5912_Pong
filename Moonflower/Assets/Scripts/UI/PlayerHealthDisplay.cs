using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthDisplay : MonoBehaviour
{
    public GameObject Flower;
    public GameObject Apple; 
    //public GameObject Anai;
    private CurrentPlayer playerInfo; 


    private bool playerIsAnai = true; 

    private LifeAppleController appleControl;
    private LifeFlowerController flowerControl;
    private CharacterStats anaiStats;
    private CharacterStats mimbiStats;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GameObject.Find("Player").GetComponent<CurrentPlayer>(); 
        appleControl = new LifeAppleController(Apple);
        flowerControl = new LifeFlowerController(Flower);
        anaiStats = playerInfo.PlayerAnaiObj.GetComponent<CharacterStats>();
        mimbiStats = playerInfo.PlayerMimbiObj.GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (playerIsAnai != playerInfo.IsAnai())
            switchPlayer();


        if (playerInfo.IsAnai())
        {
            float healthFrac = 1.0f * anaiStats.CurrentHealth / anaiStats.MaxHealth;
            flowerControl.UpdateFlower(healthFrac);
        }
        else
        {
            float healthFrac = 1.0f * mimbiStats.CurrentHealth / mimbiStats.MaxHealth;
            appleControl.UpdateApple(healthFrac);
        } 
    }

    public void HitHealth(int current, int max)
    {
        if (playerInfo.IsAnai())
            flowerControl.Hit(); 
        else
            appleControl.Hit(); 

    }
    private void switchPlayer()
    {
        playerIsAnai = playerInfo.IsAnai();
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
