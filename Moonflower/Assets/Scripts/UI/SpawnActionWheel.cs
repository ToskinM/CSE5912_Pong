using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActionWheel : MonoBehaviour
{
    public static SpawnActionWheel current;

    public GameObject ActionWheelPrefab;
    public GameObject GameStateManager;
    public GameObject InteractionPopup;
    private InteractionPopup interaction; 
    private GameStateController gameStateController;
    private FollowCamera followCamera;
    private FieldOfView currentPlayerInteractionFOV;

    private ActionWheel activeWheel;
    private ShowInspect inspect; 
    private ShowInventory inventory; 

    private GameObject target;
    private INPCController targetController;

    private FeedbackText feedback; 

    private bool wheelAvailable = true;
    private bool wheelShowing = false;
    private bool inRange = false;

    private int distractTime = 5;

    bool otherWindowUp = false; 

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

        activeWheel = ActionWheelPrefab.GetComponent<ActionWheel>();
        gameStateController = GameStateController.current;
        followCamera = LevelManager.current.mainCamera;

        inspect = GameObject.Find("HUD").GetComponent<ShowInspect>();
        inventory = GameObject.Find("HUD").GetComponent<ShowInventory>();
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
        interaction = InteractionPopup.GetComponent<InteractionPopup>();

        Debug.Log("屌你老妈哇佬");

        currentPlayerInteractionFOV = PlayerController.instance.ActivePlayerInteractionFOV;
    }

    private void OnEnable()
    {
        Debug.Log("屌你老妈");
        Debug.Log(currentPlayerInteractionFOV == null);
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
            else
            {
                target = null;
                targetController = null;
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
        otherWindowUp = otherWindowUp || PlayerController.instance.TalkingPartner != null; 

        if (followCamera != LevelManager.current.mainCamera)
        {
            followCamera = LevelManager.current.mainCamera;
        }
        if (!otherWindowUp)
        {
            float dist = float.MaxValue; 
            if(target)
                dist = Vector3.Distance(target.transform.position, LevelManager.current.currentPlayer.transform.position);
            if (target && dist <= activationRange)
            {
                if (!wheelShowing)
                    interaction.EnableNPC(dist);
                DetectInteraction();
            }
            else
            {
                interaction.DisableNPC(); 
            }
        }
        else
        {
            interaction.DisableNPC();
            otherWindowUp = inspect.Shown || inventory.Shown; 
        }
    }

    private void HandleWheelSelection(int selection)
    {
        HideWheel();
        //Debug.Log(selection);

        switch (selection)
        {
            case 0:

                string targetName = targetController.Inspect();
                inspect.Show(targetName);
                otherWindowUp = true;
                break;
            case 1:
                if (PlayerController.instance.GetActiveCharacter() != PlayerController.PlayerCharacter.Mimbi)
                {
                    targetController.Talk();
                    followCamera.LockOff();
                }
                else
                    feedback.ShowText("Sorry " + targetController.Inspect() + " don't understand Mimbi" );
                break;
            case 2:
                if (PlayerController.instance.GetActiveCharacter() == PlayerController.PlayerCharacter.Mimbi)
                {
                    targetController.Distract(PlayerController.instance.GetActivePlayerObject());
                    StartCoroutine( DistractionEnd(distractTime, targetController));
                    PlayerController.instance.GetComponent<PlayerController>().StartMimbiDistraction();
                    interaction.NotAllowed = true;
                }
                else
                {
                    feedback.ShowText("Seems like nothing happen");
                }
                break;
            case 3:
                if (inventory.HasInv())
                {
                    inventory.ShowGiftInventory(targetController);
                    otherWindowUp = true; 
                }
                else
                    feedback.ShowText("You have nothing to give."); 

                //targetController.Gift("none");
                break;
            default:
                break;
        }
    }

    IEnumerator DistractionEnd(float time, INPCController tController)
    {
        yield return new WaitForSeconds(time);
        PlayerController.instance.GetComponent<PlayerController>().EndMimbiDistraction();
        //PlayerController.instance.GetComponent<PlayerAnimatorController>().DisableDistraction();
        tController.EndDistract();
        interaction.NotAllowed = false;
    }



    public void DetectInteraction()
    {
        if (Input.GetButtonDown("Interact") )
        {
            ShowWheel();
        }
        else if (Input.GetButtonUp("Interact"))
        {
            HideWheel();
        }

    }

    private void ShowWheel()
    {
        //gameStateController.SetMouseLock(false);
        GameStateController.current.PauseGame();
        interaction.DisableNPC();
        activeWheel.gameObject.SetActive(true);
        wheelShowing = true;

        activeWheel.OnSelectOption += HandleWheelSelection;
        GameStateController.current.SetPlayerFrozen(true);
        feedback.KillText();
    }
    private void HideWheel()
    {
        //gameStateController.SetMouseLock(true);

        activeWheel.gameObject.SetActive(false);
        wheelShowing = false;
        GameStateController.current.UnpauseGame();

        activeWheel.OnSelectOption -= HandleWheelSelection;
        GameStateController.current.SetPlayerFrozen(false);
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
