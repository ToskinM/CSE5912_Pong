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
        GameStateController.OnFreezePlayer += HandleFreezeEvent;
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
            gameStateController.SetMouseLock(false);
            gameStateController.PauseGame();
            activeWheel = Instantiate(ActionWheelPrefab, Input.mousePosition, Quaternion.identity, transform);
        }
        else if (Input.GetButtonUp("Interact"))
        {
            gameStateController.SetMouseLock(true);
            Destroy(activeWheel);
            gameStateController.UnpauseGame();
        }
    }

    public void HandleFreezeEvent(bool frozen)
    {
        enabled = !frozen;
    }
}
