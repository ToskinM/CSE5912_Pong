using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActionWheel : MonoBehaviour
{
    public GameObject ActionWheelPrefab;
    public GameObject GameStateManager;
    private GameStateController gameStateController;
    private GameObject activeWheel;

    // Start is called before the first frame update
    void Start()
    {
        gameStateController = GameStateManager.GetComponent<GameStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectInteraction();
    }

    public void DetectInteraction()
    {
        if (Input.GetButtonDown("Interact"))
        {
            gameStateController.PauseGame();
            activeWheel = Instantiate(ActionWheelPrefab, Input.mousePosition, Quaternion.identity, transform);
        }
        else if (Input.GetButtonUp("Interact"))
        {
            Destroy(activeWheel);
            gameStateController.unPauseGame();
        }
    }
}
