using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthDisplay : MonoBehaviour
{
    public GameObject Flower;
    public GameObject Apple;
    public GameObject PlayerObject;

    private PlayerController playerController;

    private LifeAppleController appleControl;
    private LifeFlowerController flowerControl;
    private CharacterStats playerStats;
    //private CharacterStats anaiStats;
    //private CharacterStats mimbiStats;

    void Awake()
    {
        playerController = PlayerObject.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        appleControl = new LifeAppleController(Apple);
        flowerControl = new LifeFlowerController(Flower);
        playerStats = PlayerObject.GetComponent<CharacterStats>();

        PlayerController.OnCharacterSwitch += SwitchHealthBar;
    }

    void Update()
    {
        float healthFrac = 1.0f * playerStats.CurrentHealth / playerStats.MaxHealth;

        if (playerController.GetActiveCharacter() == PlayerController.PlayerCharacter.Anai)
        {
            flowerControl.UpdateFlower(healthFrac);
        }
        else
        {
            appleControl.UpdateApple(healthFrac);
        } 
    }

    public void HitHealth(int current, int max)
    {
        if (playerController.GetActiveCharacter() == PlayerController.PlayerCharacter.Anai)
            flowerControl.Hit(); 
        else
            appleControl.Hit(); 

    }

    void SwitchHealthBar(PlayerController.PlayerCharacter activeChar)
    {
        if (activeChar == PlayerController.PlayerCharacter.Anai)
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
