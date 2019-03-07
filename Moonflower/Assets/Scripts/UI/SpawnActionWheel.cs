using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActionWheel : MonoBehaviour
{
    public GameObject ActionWheelPrefab;
    public GameObject GameStateManager;
    private GameStateController gameStateController;
    private FollowCamera followCamera;

    private ActionWheel activeWheel;

    private bool wheelAvailable = true;

    void Start()
    {
        gameStateController = GameStateManager.GetComponent<GameStateController>();
        followCamera = LevelManager.current.mainCamera;
    }

    private void OnEnable()
    {
        GameStateController.OnFreezePlayer += HandleFreezeEvent;

        LevelManager.current.mainCamera.OnLockon += HandleLockonEvent;
        LevelManager.current.mainCamera.OnLockoff += HandleLockoffEvent;
    }
    private void OnDisable()
    {
        GameStateController.OnFreezePlayer -= HandleFreezeEvent;

        LevelManager.current.mainCamera.OnLockon -= HandleLockonEvent;
        LevelManager.current.mainCamera.OnLockoff -= HandleLockoffEvent;
    }

    void Update()
    {
        if (wheelAvailable)
            DetectInteraction();
    }

    private void GetNPCInfo()
    {

    }

    public void DetectInteraction()
    {
        if (Input.GetButtonDown("Interact") && activeWheel)
        {
            gameStateController.SetMouseLock(false);
            gameStateController.PauseGame();
            activeWheel.gameObject.SetActive(true);
            //activeWheel = Instantiate(ActionWheelPrefab, Input.mousePosition, Quaternion.identity, transform);
        }
        else if (Input.GetButtonUp("Interact") && activeWheel)
        {
            gameStateController.SetMouseLock(true);
            activeWheel.gameObject.SetActive(false);
            //Destroy(activeWheel);
            gameStateController.UnpauseGame();
        }
    }

    public void HandleLockonEvent(GameObject target)
    {
        INPCController targetNPC = target.GetComponent<INPCController>();
        if (targetNPC != null)
        {
            activeWheel = Instantiate(ActionWheelPrefab, Input.mousePosition, Quaternion.identity, transform).GetComponent<ActionWheel>();
            Debug.Log(targetNPC.GetType().ToString());

            activeWheel.Initialize(targetNPC.Icon);
            activeWheel.gameObject.SetActive(false);
        }
    }
    public void HandleLockoffEvent()
    {
        if (activeWheel)
        {
            Destroy(activeWheel.gameObject);
            activeWheel = null;
        }
    }

    public void HandleFreezeEvent(bool frozen)
    {
        wheelAvailable = !frozen;
    }
}
