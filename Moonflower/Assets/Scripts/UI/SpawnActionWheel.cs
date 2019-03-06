using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActionWheel : MonoBehaviour
{
    public GameObject ActionWheelPrefab;
    public GameObject GameStateManager;
    private GameStateController gameStateController;
    private GameObject activeWheel;

    void Start()
    {
        gameStateController = GameStateManager.GetComponent<GameStateController>();
    }

    private void OnEnable()
    {
        GameStateController.OnFreezePlayer += HandleFreezeEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnFreezePlayer -= HandleFreezeEvent;
    }

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
