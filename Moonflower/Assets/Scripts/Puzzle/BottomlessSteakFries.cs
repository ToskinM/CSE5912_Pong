using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomlessSteakFries : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        player.transform.position = new Vector3(230, 22, 265);

        if (other.gameObject == PlayerController.instance.GetActivePlayerObject())
        {
            PlayerController.instance.ActivePlayerStats.TakeDamage(10, "PlatformLava");
        }
    }
}
