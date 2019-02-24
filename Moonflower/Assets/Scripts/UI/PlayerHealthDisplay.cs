using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthDisplay : MonoBehaviour
{
    public GameObject Flower;
    public GameObject Apple; 
    public GameObject Anai; 

    public bool Dead { get { return appleControl.Dead || flowerControl.Dead; } }

    private bool playerIsAnai = true; 

    private LifeAppleController appleControl;
    private LifeFlowerController flowerControl;
    private AnaiController anaiController; 

    // Start is called before the first frame update
    void Start()
    {
        appleControl = new LifeAppleController(Apple);
        flowerControl = new LifeFlowerController(Flower);
        anaiController = Anai.GetComponent<AnaiController>();

    }

    void Update()
    {
        if (playerIsAnai && !anaiController.Playing)
            switchPlayer();

        if (!playerIsAnai && anaiController.Playing)
            switchPlayer(); 

        if (playerIsAnai)
            flowerControl.UpdateFlower();
        else
            appleControl.UpdateApple(); 
    }

    public void HitHealth(int current, int max)
    {
        if (playerIsAnai)
            flowerControl.HitHealth(current,max);
        else
            appleControl.HitHealth(current,max); 

    }

    public void Heal()
    {
        if (playerIsAnai)
            flowerControl.HealPetal();
        else
            appleControl.HealApple(); 
    }

    private void switchPlayer()
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
