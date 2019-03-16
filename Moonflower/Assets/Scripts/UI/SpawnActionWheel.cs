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
    private FieldOfView currentPlayerInteractionFOV;

    private ActionWheel activeWheel;

    private ShowInventory inventory; 

    private GameObject target;
    private INPCController targetController;

    private FeedbackText feedback; 

    private bool wheelAvailable = true;
    private bool wheelShowing = false;
    private bool inRange = false;

    private float activationRange = 5f;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }

        currentPlayerInteractionFOV = PlayerController.instance.ActivePlayerInteractionFOV;
    }

    void Start()
    {
        activeWheel = ActionWheelPrefab.GetComponent<ActionWheel>(); 
        gameStateController = GameStateController.current;
        followCamera = LevelManager.current.mainCamera;


        inventory = GameObject.Find("HUD").GetComponent<ShowInventory>(); 
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
    }

    private void OnEnable()
    {
        currentPlayerInteractionFOV.OnNewClosestTarget += HandleInteractionFOVTargetUpdate;

        PlayerController.OnCharacterSwitch += SwitchInteractionFOV;

        followCamera = LevelManager.current.mainCamera;
        GameStateController.OnFreezePlayer += HandleFreezeEvent;

        //followCamera.OnLockon += HandleLockonEvent;
        //followCamera.OnLockoff += HandleLockoffEvent;
    }
    private void OnDisable()
    {
        currentPlayerInteractionFOV.OnNewClosestTarget -= HandleInteractionFOVTargetUpdate;

        PlayerController.OnCharacterSwitch -= SwitchInteractionFOV;
        GameStateController.OnFreezePlayer -= HandleFreezeEvent;

        //followCamera.OnLockon -= HandleLockonEvent;
        //followCamera.OnLockoff -= HandleLockoffEvent;
    }

    private void SwitchInteractionFOV(PlayerController.PlayerCharacter activeChar)
    {
        currentPlayerInteractionFOV.OnNewClosestTarget -= HandleInteractionFOVTargetUpdate;
        currentPlayerInteractionFOV = PlayerController.instance.ActivePlayerInteractionFOV;
        currentPlayerInteractionFOV.OnNewClosestTarget += HandleInteractionFOVTargetUpdate;
    }
    private void HandleInteractionFOVTargetUpdate(GameObject closestNPC)
    {
        target = closestNPC;
        if (target != null)
        {
            INPCController targetNPC = target.GetComponent<INPCController>();
            if (targetNPC != null)
            {
                targetController = targetNPC;

                activeWheel.Initialize(targetNPC.icon, targetNPC.actionsAvailable);
                activeWheel.gameObject.SetActive(false);
            }
        }
        else
        {
            targetController = null;
        }
    }

    void Update()
    {
        currentPlayerInteractionFOV = PlayerController.instance.ActivePlayerInteractionFOV;

        if (followCamera != LevelManager.current.mainCamera)
        {
            followCamera = LevelManager.current.mainCamera;
        }

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
                if (inventory.HasInv())
                {
                    inventory.ShowGiftInventory(targetController);
                    interactionPopup.SetActive(false);
                }
                else
                    feedback.ShowText("You have nothing to give."); 

                //targetController.Gift("none");
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

            activeWheel.Initialize(targetNPC.icon, targetNPC.actionsAvailable);
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
