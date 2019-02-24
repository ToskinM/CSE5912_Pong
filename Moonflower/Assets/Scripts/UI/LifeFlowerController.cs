using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class LifeFlowerController : MonoBehaviour
{
    public GameObject Flower;
    //public GameObject Player;

    public bool Dead = false;

    private bool healing = false;
    private bool falling = false; 
    private enum petalState { full, down1, down2, down3, gone}

    //private CharacterStats playerStat;
    //private TextMeshProUGUI healthText;
    private List<Image> petals;
    private Dictionary<Image, petalState> stateOfPetal; 
    private Image currPetal;
    private Image fallingPetal;
    private bool fallLeft = true;  
    private PetalFactory petalFactory;


    private bool damageDelt = false;
    private int shakeCount = 0;
    private int maxShake = 20;
    private int healCount = 0; 
    private Vector3 initPosition; 


    // Start is called before the first frame update
    public LifeFlowerController(GameObject flowerOb)
    {
        Flower = flowerOb;
        //Player = playerOb; 
        //playerStat = Player.GetComponent<CharacterStats>();
       //healthText = gameObject.GetComponent<TextMeshProUGUI>();
        petalFactory = new PetalFactory();

        petals = new List<Image>();
        stateOfPetal = new Dictionary<Image, petalState>();
        for(int i = 0; i < Flower.transform.childCount - 1; i++)
        {
            Image petal = Flower.transform.GetChild(i).GetComponent<Image>();

            petals.Add(petal);
            stateOfPetal.Add(petal, petalState.full); 
        }
        currPetal = petals[0];

        initPosition = Flower.transform.position;

    }

    public void UpdateFlower()
    {
        if(damageDelt)
        {
            if (shakeCount < maxShake)
            {
                if (shakeCount < 5 || (shakeCount > 10 && shakeCount < 15))
                {
                    Flower.transform.position = new Vector3(Flower.transform.position.x - 0.3f, Flower.transform.position.y);
                }
                else
                {
                    Flower.transform.position = new Vector3(Flower.transform.position.x + 0.3f, Flower.transform.position.y);

                }
                shakeCount++; 
            }
            else
            {
                shakeCount = 0;
                damageDelt = false;
                Flower.transform.position = initPosition; 
            }
        }
        if(healing)
        {
            petalState currState = stateOfPetal[currPetal];
            if (currState != petalState.full)
            {
                if (healCount % 10 == 0)
                {
                    petalState newState = currState-1;
                    currPetal.sprite = getPetal(newState); 
                    updatePetalState(currPetal, newState);
                }
                healCount++; 
            }
            else
            {
                healing = false;
                healCount = 0; 
            }
        }
        if(falling)
        {
            fallingPetal.transform.position -= new Vector3(0, 1);
            fallingPetal.color -= new Color(0, 0, 0, 0.02f);
            if(fallLeft)
                fallingPetal.transform.RotateAround(fallingPetal.transform.position, Vector3.forward, 80*Time.deltaTime);
            else
                fallingPetal.transform.RotateAround(fallingPetal.transform.position,-Vector3.forward, 80 * Time.deltaTime);

            if (fallingPetal.color.a <= 0)
            {
                Destroy(fallingPetal.gameObject);
                falling = false; 
            }
        }
    }

    public void HitHealth()
    {
        if (!Dead)
        {
            damageDelt = true;
            damagePetal();
        }

    }

    public void HealPetal()
    {
        if (stateOfPetal[currPetal] == petalState.full)
        {
            int newIndex = petals.IndexOf(currPetal) - 1;
            if (newIndex >= 0)
            {
                currPetal = petals[newIndex];
                currPetal.gameObject.SetActive(true);
            }
        }
        healing = true;
    }

    private Sprite getPetal(petalState p)
    {
        switch(p)
        {
            case petalState.full:
                return petalFactory.GetHealthyPetal();
            case petalState.down1:
                return petalFactory.GetDecayPetal1();
            case petalState.down2:
                return petalFactory.GetDecayPetal2();
            case petalState.down3:
                return petalFactory.GetDecayPetal3();
            default:
                return null; 
        }
    }

    private void damagePetal()
    {
        Image petal = currPetal; 
        switch (stateOfPetal[petal])
        {
            case petalState.full:
                petal.sprite = petalFactory.GetDecayPetal1();
                updatePetalState(petal, petalState.down1); 
                break;
            case petalState.down1:
                petal.sprite = petalFactory.GetDecayPetal2();
                updatePetalState(petal, petalState.down2);
                break;
            case petalState.down2:
                petal.sprite = petalFactory.GetDecayPetal3();
                updatePetalState(petal, petalState.down3);
                break;
            case petalState.down3:
                cloneFallingPetal(petal); 
                falling = true; 
                petal.gameObject.SetActive(false); 
                updatePetalState(petal, petalState.gone);
                int newIndex = petals.IndexOf(petal) + 1;
                if (newIndex < petals.Count)
                    currPetal = petals[newIndex];
                else
                    Dead = true; 
                break;
        }
    }

    private void cloneFallingPetal(Image petal)
    {
        GameObject petalOb = Instantiate(petal.gameObject, Flower.transform);
        petalOb.transform.position = petal.transform.position;
        petalOb.transform.rotation = petal.transform.rotation;
        fallingPetal = petalOb.GetComponent<Image>();
        fallLeft = petals.IndexOf(petal) <= 1; 
    }


    private void updatePetalState(Image petal, petalState state)
    {
        if(stateOfPetal.ContainsKey(petal))
        {
            stateOfPetal.Remove(petal);
            stateOfPetal.Add(petal, state); 
        }
        else
        {
            stateOfPetal.Add(petal, state);
        }
    }


}
