using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupArea : MonoBehaviour
{
    public CharacterStats Stats { get; private set; }
    public bool Pickable;

    public GameObject pickupArea;

    //private const string PICKUP_AXIS = "Pickup";
    // Start is called before the first frame update
    void Start()
    {
        Pickable = false;
        pickupArea.SetActive(false);
    }

    public void DecidePickable()
    {
        if (Input.GetButton("Pickup"))
        {
            pickupArea.SetActive(true);
            Pickable = true;
        }
        if (Input.GetButtonUp("Pickup"))
        {
            pickupArea.SetActive(false);
            Pickable = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
    // Update is called once per frame
    void Update()
    {
        DecidePickable();
    }
}
