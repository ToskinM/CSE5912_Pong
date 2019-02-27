using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System; 


public class LifeAppleController : MonoBehaviour
{
    public GameObject Apple;

    private Image appleImage; 
    private enum appleState { full, rotting, dead}

    private List<Sprite> apples;
    private appleState currState = appleState.full;
    private appleState goalState = appleState.full; 
    private int goalRotIndex = -1; 
    private int currRotIndex = -1;
    private AppleFactory factory;


    private bool damageDelt = false;
    private int shakeCount = 0;
    private int maxShake = 20;
    private int healCount = 0; 
    private Vector3 initPosition; 


    // Start is called before the first frame update
    public LifeAppleController(GameObject flowerOb)
    {
        Apple = flowerOb;
        appleImage = Apple.GetComponent<Image>(); 
        factory = new AppleFactory();

        apples = new List<Sprite>();

        initPosition = Apple.transform.position;

        apples.Add(factory.GetHealthyApple());
        apples.Add(factory.GetRot1());
        apples.Add(factory.GetRot2());
        apples.Add(factory.GetRot3());
        apples.Add(factory.GetRot4());
        apples.Add(factory.GetRot5());
        apples.Add(factory.GetRot6());
        apples.Add(factory.GetRot7());
        apples.Add(factory.GetRot8());
        apples.Add(factory.GetRot9());
        apples.Add(factory.GetRot10());
        apples.Add(factory.GetRot11());
        apples.Add(factory.GetRot12());
        apples.Add(factory.GetRot13());
        apples.Add(factory.GetRot14());
        apples.Add(factory.GetRot15());
        apples.Add(factory.GetRot16());
        apples.Add(factory.GetRot17());
        apples.Add(factory.GetRot18());
        apples.Add(factory.GetDeadApple()); 

    }

    public void UpdateApple(float healthFrac)
    {
        updateState(healthFrac);
        //graphic needs changing
        if(goalRotIndex!= currRotIndex)
        {
            //too healthy
            if(currRotIndex < goalRotIndex)
            {
                rotApple();  

            }
            //too rotten
            else
            {
                healApple(); 
            }

        }
        else
        {
            healCount = 0; 
        }

        //hit shake 
        if (damageDelt)
        {
            if (shakeCount < maxShake)
            {
                if (shakeCount < 5 || (shakeCount > 10 && shakeCount < 15))
                {
                    Apple.transform.position = new Vector3(Apple.transform.position.x - 0.3f, Apple.transform.position.y);
                }
                else
                {
                    Apple.transform.position = new Vector3(Apple.transform.position.x + 0.3f, Apple.transform.position.y);

                }
                shakeCount++; 
            }
            else
            {
                shakeCount = 0;
                damageDelt = false;
                Apple.transform.position = initPosition; 
            }
        }
    }

    private void rotApple()
    {
        currRotIndex = goalRotIndex;
        if (currRotIndex >= apples.Count)
        {
            appleImage.sprite = apples[apples.Count - 1];
            currRotIndex = apples.Count - 1; 
        }
        else
        {
            appleImage.sprite = apples[currRotIndex];
        }
    }

    private void healApple()
    {
        if (healCount % 6 == 0)
        {
            currRotIndex--;
            if (currRotIndex < 0)
            {
                currRotIndex = 0;
            }
            appleImage.sprite = apples[currRotIndex];

        }
        healCount++;
    }

    private void updateState(float healthFrac)
    {
        int index = (int)Math.Round(healthFrac * apples.Count);
        goalRotIndex = apples.Count - index;

    }

    public void Hit()
    {
        damageDelt = true; 

    }

}
