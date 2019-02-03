using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthDisplay : MonoBehaviour
{
    public GameObject Player;

    private CharacterStats playerStat;
    private TextMeshProUGUI healthText;

    // TODO: Replace text with flower symbols

    // Start is called before the first frame update
    void Start()
    {
        playerStat = Player.GetComponent<CharacterStats>();
        healthText = gameObject.GetComponent<TextMeshProUGUI>();

        RefreshHealthDisplay();
    }

    // TODO: Remove after combat features are implemented, call RefreshHealthDisplay() instead to reduce number of Update() calls
    private void Update()
    {
        RefreshHealthDisplay();
    }

    public void RefreshHealthDisplay()
    {
        string displayText = "Health: " + playerStat.CurrentHealth.ToString();
        healthText.SetText(displayText);
    }
}
