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
    private List<Image> healingPetals = new List<Image>();
    private Dictionary<Image, petalState> healingState = new Dictionary<Image, petalState>();
    private Dictionary<Image, int> healingCount = new Dictionary<Image, int>(); 
    private List<Image> fallingPetals = new List<Image>(); 
    private Dictionary<Image, petalState> stateOfPetal; 
    private Image currPetal;
    Dictionary<Image, bool> fallLeft = new Dictionary<Image, bool>(); 
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
            List<Image> toRemove = new List<Image>();
            foreach (Image healingPetal in healingPetals)
            {
                petalState currState = stateOfPetal[healingPetal];
                if (currState != petalState.full)
                {
                    int count = healingCount[healingPetal];
                    if (count % 7 == 0)
                    {
                        petalState newState = currState - 1;
                        healingPetal.sprite = getPetal(newState);
                        updateHealPetalState(currPetal, newState);
                    }
                    //replace count
                    healingCount.Remove(healingPetal);
                    count++;  
                    healingCount.Add(healingPetal,count);
                }
                else
                {
                    toRemove.Add(healingPetal); 
                }
            }

            while(toRemove.Count > 0)
            {
                Image d = toRemove[0];
                toRemove.Remove(d);
                healingPetals.Remove(d);
            }
            if (healingPetals.Count == 0)
                healing = false;
        }
        if(falling)
        {
            List<Image> toDestroy = new List<Image>(); 
            foreach (Image fallingPetal in fallingPetals)
            {
                fallingPetal.transform.position -= new Vector3(0, 1);
                fallingPetal.color -= new Color(0, 0, 0, 0.02f);
                if (fallLeft[fallingPetal])
                    fallingPetal.transform.RotateAround(fallingPetal.transform.position, Vector3.forward, 80 * Time.deltaTime);
                else
                    fallingPetal.transform.RotateAround(fallingPetal.transform.position, -Vector3.forward, 80 * Time.deltaTime);

                if (fallingPetal.color.a <= 0)
                {
                    fallLeft.Remove(fallingPetal);
                    //Destroy(fallingPetal.gameObject);
                    toDestroy.Add(fallingPetal); 
                }
            }

            while(toDestroy.Count > 0)
            {
                Image d = toDestroy[0];
                toDestroy.Remove(d);
                fallingPetals.Remove(d);
                Destroy(d.gameObject);  
            }

            if (fallingPetals.Count == 0)
                falling = false;
        }
        
    }

    public void HitHealth(int current, int max)
    {
        if (!Dead)
        {
            damageDelt = true;
            float fracHealth = 1.0f * current / max;
            float newFrac;
            if(fracHealth > 4f/5f)
            {
                currPetal = petals[0];
                newFrac = fracHealth - 4f/5f;
            }
            else if (fracHealth > 3f/5f)
            {
                currPetal = petals[1];
                newFrac = fracHealth - 3f / 5f;
            }
            else if(fracHealth > 2f/5f)
            {
                currPetal = petals[2];
                newFrac = fracHealth - 2f / 5f;
            }
            else if (fracHealth > 1f/5f)
            {
                currPetal = petals[3];
                newFrac = fracHealth - 1f / 5f;
            }
            else
            {
                currPetal = petals[4];
                newFrac = fracHealth; 
            }

            newFrac *= 5; 
            damagePetal(newFrac);
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
                healingState.Add(currPetal, stateOfPetal[currPetal]);
                updatePetalState(currPetal, petalState.full);
                healingPetals.Add(currPetal);
                healingCount.Add(currPetal, 0); 
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

    private void damagePetal(float frac)
    {
        for(int i = 0; i < petals.IndexOf(currPetal); i++)
        {
            if(petals[i].gameObject.active)
            {
                petals[i].gameObject.SetActive(false); 
            }
        }

        Image petal = currPetal; 
        if(frac < 1f/5f)
        {
            petal.sprite = petalFactory.GetDecayPetal3(); 
            cloneFallingPetal(petal);
            falling = true; 
            petal.gameObject.SetActive(false); 
            updatePetalState(petal, petalState.gone);
            int newIndex = petals.IndexOf(petal) + 1;
            if (newIndex < petals.Count)
                currPetal = petals[newIndex];
            else
                Dead = true; 
        }
        else if(frac < 2f/5f)
        {
            petal.sprite = petalFactory.GetDecayPetal3();
            updatePetalState(petal, petalState.down3);
        }
        else if(frac < 3f/5f)
        {
            petal.sprite = petalFactory.GetDecayPetal2();
            updatePetalState(petal, petalState.down2);
        }
        else if(frac < 4f/5f)
        {
            petal.sprite = petalFactory.GetDecayPetal1();
            updatePetalState(petal, petalState.down1); 
        }
        else 
        {
            petal.sprite = petalFactory.GetHealthyPetal();
            updatePetalState(petal, petalState.full);
        }

    }

    private void cloneFallingPetal(Image petal)
    {
        GameObject petalOb = Instantiate(petal.gameObject, Flower.transform);
        petalOb.transform.position = petal.transform.position;
        petalOb.transform.rotation = petal.transform.rotation;
        Image fallingPetal = petalOb.GetComponent<Image>();
        fallingPetals.Add(fallingPetal); 
        fallLeft.Add(fallingPetal, petals.IndexOf(petal) <= 1); 
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

    private void updateHealPetalState(Image petal, petalState state)
    {
        if (healingState.ContainsKey(petal))
        {   
            healingState.Remove(petal);
            healingState.Add(petal, state);
        }
        else
        {
            healingState.Add(petal, state);
        }
    }

}
