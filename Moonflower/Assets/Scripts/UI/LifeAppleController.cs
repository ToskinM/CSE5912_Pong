using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class LifeAppleController : MonoBehaviour
{
    public GameObject Apple;

    public bool Dead = false;

    private Image appleImage; 
    private bool healing = false;
    private int healIndex = 0; 
    private int healIncrementNum = 4; 
    private enum appleState { full, rotting, dead}

    private List<Sprite> apples;
    private appleState currState = appleState.full;
    private int currRotIndex = 0;
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

    }

    public void UpdateApple()
    {
        if(damageDelt)
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
        if(healing)
        {
            if (healCount % 6 == 0)
            {
                currRotIndex--;
                if (currRotIndex < 0)
                {
                    currRotIndex = 0;
                    currState = appleState.full;
                    appleImage.sprite = factory.GetHealthyApple();
                }
                else
                {
                    appleImage.sprite = apples[currRotIndex];
                }
                healIndex++;
            }
            healCount++;
            //healing = currState != appleState.full && currRotIndex != 3 && currRotIndex != 8 && currRotIndex != 13;
            healing = currState != appleState.full && healIndex < healIncrementNum;
            if (!healing)
            {
                healCount = 0;
                healIndex = 0; 
            }
        }
    }

    public void HitHealth()
    {
        if (!Dead)
        {
            if(currState == appleState.full)
            {
                appleImage.sprite = apples[currRotIndex];
                currState = appleState.rotting; 
            }
            else if(currRotIndex == apples.Count - 1)
            {
                appleImage.sprite = factory.GetDeadApple();
                Dead = true;
            }
            else
            {
                currRotIndex++;
                appleImage.sprite = apples[currRotIndex];
            }
            damageDelt = true;
        }

    }

    public void HealApple()
    {
        if (currState != appleState.full)
        {
            healing = true;
            healCount = 0;
        }

    }



}
