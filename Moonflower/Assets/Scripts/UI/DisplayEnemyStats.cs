using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayEnemyStats : MonoBehaviour
{
    TextMesh[] valueTexts;
    StealthDetection stealthDetection;
    CharacterStats health;

    // Start is called before the first frame update
    void Start()
    {
        valueTexts = GetComponentsInChildren<TextMesh>();
        stealthDetection = GetComponent<StealthDetection>();
        health = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        valueTexts[0].text = "Sus: " + stealthDetection?.awarenessMeter.ToString();
        valueTexts[1].text = "Hp: " + health.CurrentHealth.ToString();
    }
}
