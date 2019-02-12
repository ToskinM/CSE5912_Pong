using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHealthDisplay : MonoBehaviour
{
    public GameObject Flower;
    public GameObject Player;

    public bool Dead = false; 

    private enum petalState { full, down1, down2, down3, gone}

    private CharacterStats playerStat;
    private TextMeshProUGUI healthText;
    private List<Image> petals;
    private Dictionary<Image, petalState> stateOfPetal; 
    private Image currPetal;
    private PetalFactory factory; 


    private bool damageDelt = false;
    private int shakeCount = 0;
    private int maxShake = 20;
    private Vector3 initPosition; 


    // Start is called before the first frame update
    void Start()
    {
        playerStat = Player.GetComponent<CharacterStats>();
        healthText = gameObject.GetComponent<TextMeshProUGUI>();
        factory = new PetalFactory(); 

        //RefreshHealthDisplay();

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

    // TODO: Remove after combat features are implemented, call RefreshHealthDisplay() instead to reduce number of Update() calls
    private void Update()
    {
        //RefreshHealthDisplay();

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
    }

    public void RefreshHealthDisplay()
    {
        //string displayText = "Health: " + playerStat.CurrentHealth.ToString();
        //healthText.SetText(displayText);
    }

    public void HitHealth()
    {

        if (!Dead)
        {
            damageDelt = true;
            damagePetal();
        }

    }

    private void damagePetal()
    {
        Image petal = currPetal; 
        switch (stateOfPetal[petal])
        {
            case petalState.full:
                petal.sprite = factory.GetDecayPetal1();
                updatePetalState(petal, petalState.down1); 
                break;
            case petalState.down1:
                petal.sprite = factory.GetDecayPetal2();
                updatePetalState(petal, petalState.down2);
                break;
            case petalState.down2:
                petal.sprite = factory.GetDecayPetal3();
                updatePetalState(petal, petalState.down3);
                break;
            case petalState.down3:
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
