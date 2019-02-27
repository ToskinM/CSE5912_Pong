using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class LifeFlowerController : MonoBehaviour
{
    public GameObject Flower;
    //public GameObject Player;

    private enum petalState { full, down1, down2, down3, gone}

    //private CharacterStats playerStat;
    //private TextMeshProUGUI healthText;
    private List<Image> petals;
    private Dictionary<Image, List<petalState>> healingState = new Dictionary<Image, List<petalState>>();
    private Dictionary<Image, int> healingCount = new Dictionary<Image, int>(); 
    private List<Image> fallingPetals = new List<Image>(); 
    private Dictionary<Image, petalState> stateOfPetal; 
    Dictionary<Image, bool> fallLeft = new Dictionary<Image, bool>(); 
    private PetalFactory petalFactory;

    //for hit shake
    private bool damageDelt = false;
    private int shakeCount = 0;
    private  const int maxShake = 24; 
    private Vector3 initPosition;

    int currPetalIndex = 0;

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
            healingCount.Add(petal, 0); 
            stateOfPetal.Add(petal, petalState.full); 
        }

        initPosition = Flower.transform.position;

    }



    public void UpdateFlower(float healthFraction)
    {
        float newFrac = determineCurrentPetal(healthFraction); //currPetalIndex is now updated
        //Debug.Log("Current index " + currPetalIndex);

        for (int i = 0; i < petals.Count; i++)
        {
            Image petal = petals[i];
            petalState state = stateOfPetal[petal];

            if (state == petalState.gone)
                petal.gameObject.SetActive(false);
            else
                petal.gameObject.SetActive(true); 


            //petal needs to heal
            if (i > currPetalIndex && state != petalState.full)
            {
                //Debug.Log("Petal " + i + " needs heal to " + (state-1)); 

                //if at right interval for animation
                int count = healingCount[petal];
                if (count % 7 == 0)
                {
                    increasePetalState(petal); 
                }

                //increment heal animation count
                count++;
                replaceHealCount(petal, count);
            }
            //petal needs death
            else if (currPetalIndex > i && state != petalState.gone)
            {
                //Debug.Log("Petal " + i + " needs death");
                setFallingPetal(petal); 
            }
            //petal is current petal
            else if(currPetalIndex == i)
            {
                petalState newState = getNewState(newFrac);
                if (stateOfPetal[petal] != newState)
                {
                    //petal is too decayed
                    if (stateOfPetal[petal] > newState)
                    {
                        //Debug.Log("Petal " + i + " needs heal");
                        int count = healingCount[petal];
                        if (count % 7 == 0)
                        {
                            increasePetalState(petal);
                            //Debug.Log("Petal " + i + " healed to " + stateOfPetal[petal]);
                        }

                        //increment heal animation count
                        count++; 
                        replaceHealCount(petal, count);
                    }
                    //petal is too healthy
                    else 
                    {
                        //Debug.Log("Petal " + i + " needs decay");
                        if (newState == petalState.gone)
                        {
                            setFallingPetal(petal); 
                        }
                        else
                        {
                            petal.sprite = getPetal(newState);
                            updatePetalState(petal, newState);
                        }


                    }
                }
            }
        }

        //make dead petals petals fall and fade
        if (fallingPetals.Count > 0)
        {
            List<Image> toDestroy = new List<Image>(); 
            foreach (Image fallingPetal in fallingPetals)
            {
                //Debug.Log("petal falling"); 
                //move down and fade
                fallingPetal.transform.position -= new Vector3(0, 1);
                fallingPetal.color -= new Color(0, 0, 0, 0.02f);

                //rotate in appropriate direction
                if (fallLeft[fallingPetal])
                    fallingPetal.transform.RotateAround(fallingPetal.transform.position, Vector3.forward, 80 * Time.deltaTime);
                else
                    fallingPetal.transform.RotateAround(fallingPetal.transform.position, -Vector3.forward, 80 * Time.deltaTime);

                //if fully faded, destroy 
                if (fallingPetal.color.a <= 0)
                {
                    fallLeft.Remove(fallingPetal);
                    toDestroy.Add(fallingPetal); 
                }
            }

            //destroy petals that need destroying
            while(toDestroy.Count > 0)
            {
                Image d = toDestroy[0];
                toDestroy.Remove(d);
                fallingPetals.Remove(d);
                Destroy(d.gameObject);  
            }
        }

        //shake flower icon when hit
        if (damageDelt)
        {
            if (shakeCount < maxShake)
            {
                int shakeSegment = maxShake / 4; 
                if (shakeCount < shakeSegment || (shakeCount > shakeSegment*2 && shakeCount < shakeSegment*3))
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

    }

    public void Hit()
    {
        damageDelt = true; 
    }

    private float determineCurrentPetal(float fracHealth)
    {
        float newFrac;
        if (fracHealth > 4f / 5f)
        {
            currPetalIndex = 0; 
            newFrac = fracHealth - 4f / 5f;
        }
        else if (fracHealth > 3f / 5f)
        {
            currPetalIndex = 1;
            newFrac = fracHealth - 3f / 5f;
        }
        else if (fracHealth > 2f / 5f)
        {
            currPetalIndex = 2;
            newFrac = fracHealth - 2f / 5f;
        }
        else if (fracHealth > 1f / 5f)
        {
            currPetalIndex = 3;
            newFrac = fracHealth - 1f / 5f;
        }
        else
        {
            currPetalIndex = 4;
            newFrac = fracHealth;
        }

        newFrac *= 5;
        return newFrac; //return new health frac of particular petal 
    }

    private petalState getNewState(float frac)
    {
        petalState newState; 
        if (frac < 1f / 5f)
        {
            newState = petalState.gone;
        }
        else if (frac < 2f / 5f)
        {
            newState = petalState.down3;
        }
        else if (frac < 3f / 5f)
        {
            newState = petalState.down2;
        }
        else if (frac < 4f / 5f)
        {
            newState = petalState.down1;
        }
        else
        {
            newState = petalState.full;
        }
        return newState; 
    }

    //do all setup to add petal to falling list 
    private void setFallingPetal(Image petal)
    {
        //made real petal dead
        petal.sprite = getPetal(petalState.down3);
        updatePetalState(petal, petalState.gone);

        //clone petal and add to falling petal list 
        GameObject petalOb = Instantiate(petal.gameObject, Flower.transform);
        petalOb.transform.position = petal.transform.position;
        petalOb.transform.rotation = petal.transform.rotation;
        petalOb.SetActive(true); 
        Image fallingPetal = petalOb.GetComponent<Image>();
        fallingPetals.Add(fallingPetal);
        fallLeft.Add(fallingPetal, petals.IndexOf(petal) <= 1);
        //Debug.Log("petal " + petals.IndexOf(petal) + " has died"); 
    }



    //do all setup to add petal to healing list 
    private void setHealingPetal(Image petal, petalState goal)
    {
        //set up current and goal states for petal 
        List<petalState> currGoalStates = new List<petalState>();
        currGoalStates.Add(stateOfPetal[petal]);
        currGoalStates.Add(goal); 
        healingState.Add(petal, currGoalStates);

        //update general view of petal to be healthy
        updatePetalState(petal, petalState.full);

        //add petal to healing petals list and set heal count to 0
        healingCount.Add(petal, 0);
    }

        //get appropriate petal sprite for petal state
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

      
    //change state of petal in the petal/state map 
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

    //change current state of petal in the HEALING petal/state map 
    private void updateHealPetalState(Image petal, petalState state)
    {
        if (healingState.ContainsKey(petal))
        {
            List<petalState> states = healingState[petal];
            states.Remove(0);
            states.Insert(0, state); 
            healingState.Remove(petal);
            healingState.Add(petal, states);
        }
    }

    private void replaceHealCount(Image petal, int newCount)
    {
        healingCount.Remove(petal);
        healingCount.Add(petal, newCount);
    }

    private void resetHeal(Image petal)
    {
        healingCount.Remove(petal);
        healingCount.Add(petal, 0);
    }

    private void increasePetalState(Image petal)
    {
        //replace state of sprite
        petalState newState = stateOfPetal[petal] - 1;
        if (newState >= petalState.full)
        {
            petal.sprite = getPetal(newState);
            updatePetalState(petal, newState);
        }

        //if petal is full, reset heal animation count
        if (newState == petalState.full)
            replaceHealCount(petal, 0);
    }


}
