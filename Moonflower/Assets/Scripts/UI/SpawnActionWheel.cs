using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActionWheel : MonoBehaviour
{
    public static SpawnActionWheel current;

    public GameObject ActionWheelPrefab;
    public GameObject GameStateManager;
    public GameObject interactionPopup;
    private GameStateController gameStateController;
    private FollowCamera followCamera;

    private ActionWheel activeWheel;

    private ShowInventory inventory; 

    private GameObject target;
    private INPCController targetController;

    private bool wheelAvailable = true;
    private bool wheelShowing = false;
    private bool inRange = false;

    private float activationRange = 5f;

    void Start()
    {
        if (current == null)
        {
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }

        gameStateController = GameStateController.current;
        followCamera = LevelManager.current.mainCamera;

        inventory = GameObject.Find("HUD").GetComponent<ShowInventory>(); 
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
        {
            if (target && Vector3.Distance(target.transform.position, LevelManager.current.currentPlayer.transform.position) <= activationRange)
            {
                if (!wheelShowing)
                    interactionPopup.SetActive(true);
                DetectInteraction();
            }
            else
            {
                interactionPopup.SetActive(false);
            }
        }
    }

    private void HandleWheelSelection(int selection)
    {
        HideWheel();
        switch (selection)
        {
            case 0:
                targetController.Inspect();
                break;
            case 1:
                targetController.Talk();
                followCamera.LockOff();
                break;
            case 2:
                targetController.Distract();
                break;
            case 3:
                inventory.ShowGiftInventory(targetController); 
                targetController.Gift("none");
                break;
            default:
                break;
        }
    }

    public void DetectInteraction()
    {
        if (Input.GetButtonDown("Interact") && activeWheel)
        {
            ShowWheel();
        }
        else if (Input.GetButtonUp("Interact") && activeWheel)
        {
            HideWheel();
        }
    }

    private void ShowWheel()
    {
        //gameStateController.SetMouseLock(false);
        gameStateController.PauseGame();
        interactionPopup.SetActive(false);
        activeWheel.gameObject.SetActive(true);
        wheelShowing = true;

        activeWheel.OnSelectOption += HandleWheelSelection;
    }
    private void HideWheel()
    {
        //gameStateController.SetMouseLock(true);
        activeWheel.gameObject.SetActive(false);
        wheelShowing = false;
        gameStateController.UnpauseGame();

        activeWheel.OnSelectOption -= HandleWheelSelection;
    }

    public void HandleLockonEvent(GameObject target)
    {
        INPCController targetNPC = target.GetComponent<INPCController>();
        if (targetNPC != null)
        {
            this.target = target;
            targetController = targetNPC;
            activeWheel = Instantiate(ActionWheelPrefab, Input.mousePosition, Quaternion.identity, transform).GetComponent<ActionWheel>();

            activeWheel.Initialize(targetNPC.Icon);
            activeWheel.gameObject.SetActive(false);
        }
    }
    public void HandleLockoffEvent()
    {
        if (activeWheel)
        {
            Destroy(activeWheel.gameObject);
            target = null;
            targetController = null;
            activeWheel = null;
        }

        //interactionPopup.SetActive(false);
    }

    public void HandleFreezeEvent(bool frozen)
    {
        wheelAvailable = !frozen;
    }
}
