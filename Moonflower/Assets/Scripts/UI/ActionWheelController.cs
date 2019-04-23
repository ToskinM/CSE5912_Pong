using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.SceneManagement;

public class ActionWheelController : MonoBehaviour
{
    public static ActionWheelController current;

    public GameObject ActionWheelPrefab;
    public GameObject InteractionPopup;
    private InteractionPopup interaction;
    private GameStateController gameStateController;
    private FollowCamera followCamera;
    //private FieldOfView currentPlayerInteractionFOV;

    private ActionWheel activeWheel;
    private ShowInspect inspect;
    private ShowInventory inventory;

    private GameObject target;
    private INPCController targetController;

    private FeedbackText feedback;

    private bool wheelAvailable = true;
    private bool wheelShowing = false;
    private bool inRange = false;
    private bool outlined = false;

    private int distractTime = 5;

    bool otherWindowUp = false;

    private float activationRange = 10f;

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

        GameObject hud = GameObject.Find("HUD");
        inspect = hud.GetComponent<ShowInspect>();
        inventory = hud.GetComponent<ShowInventory>();
        feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
        if (InteractionPopup == null)
            InteractionPopup = GameObject.Find("Interaction Popup");
        interaction = hud.GetComponent<ComponentLookup>().InteractionPopup;// InteractionPopup.GetComponent<InteractionPopup>();

        //        Debug.Log("屌你老妈哇佬");

        //currentPlayerInteractionFOV = PlayerController.instance.ActivePlayerInteractionFOV;
    }

    private void OnEnable()
    {
        //Debug.Log("屌你老妈");
        //        Debug.Log(currentPlayerInteractionFOV == null);
        PlayerController.instance.ActivePlayerInteractionFOV.OnNewClosestTarget += HandleInteractionFOVTargetUpdate;

        PlayerController.OnCharacterSwitch += SwitchInteractionFOV;

        followCamera = LevelManager.current.mainCamera;
        GameStateController.OnFreezePlayer += HandleFreezeEvent;

        //followCamera.OnLockon += HandleLockonEvent;
        //followCamera.OnLockoff += HandleLockoffEvent;
    }

    private void OnDisable()
    {
        PlayerController.instance.ActivePlayerInteractionFOV.OnNewClosestTarget -= HandleInteractionFOVTargetUpdate;

        PlayerController.OnCharacterSwitch -= SwitchInteractionFOV;
        GameStateController.OnFreezePlayer -= HandleFreezeEvent;

        //followCamera.OnLockon -= HandleLockonEvent;
        //followCamera.OnLockoff -= HandleLockoffEvent;
    }

    private void SwitchInteractionFOV(PlayerController.PlayerCharacter activeChar)
    {
        //PlayerController.instance.ActivePlayerInteractionFOV.OnNewClosestTarget -= HandleInteractionFOVTargetUpdate;
        PlayerController.instance.ActivePlayerInteractionFOV.OnNewClosestTarget += HandleInteractionFOVTargetUpdate;
    }
    private void HandleInteractionFOVTargetUpdate(GameObject closestNPC)
    {
        //Debug.Log("handle target"); 
        
        if (closestNPC != null)
        {
            //Debug.Log("Got a target!"); 
            INPCController targetNPC = closestNPC.GetComponent<INPCController>();


            if (targetNPC != null)
            {
                //Debug.Log("Target has controller!"); 
                //Debug.Log("valid npc!");

                if (closestNPC != target)
                {
                    if (outlined)
                        RemoveOutlineToNPC(target);
                }

                target = closestNPC;
                targetController = targetNPC;

                activeWheel.Initialize(targetNPC.icon, targetNPC.actionsAvailable);
                activeWheel.gameObject.SetActive(false);

                if (!outlined)
                    ApplyOutlineToNPC(target);
            }
            else
            {
                //Debug.Log("No controller :("); 
                if (outlined)
                    RemoveOutlineToNPC(target);

                target = null;
                targetController = null;
            }
        }
        else
        {
            if (outlined)
                RemoveOutlineToNPC(target);

            target = null;
            targetController = null;
            //Debug.Log("No target :((");
        }
    }

    void Update()
    {
        otherWindowUp = otherWindowUp || PlayerController.instance.TalkingPartner != null;

        if (followCamera != LevelManager.current.mainCamera)
        {
            followCamera = LevelManager.current.mainCamera;
        }
        if (!otherWindowUp)
        {
            float dist = float.MaxValue;
            if (target)
                dist = Vector3.Distance(target.transform.position, PlayerController.instance.GetActivePlayerObject().transform.position);

            //if (target && dist <= activationRange)
            if (target)
            {
                if (!wheelShowing)
                {


                    interaction.EnableNPC(dist);
                    PlayerController.instance.SaveSheathState(); 
                }

                if (GetIfInteractable(targetController.actionsAvailable))
                {
                    DetectInteraction();
                }
            }
            else
            {
                PlayerController.instance.SaveSheathState();
                interaction.DisableNPC();
            }
        }
        else
        {
            interaction.DisableNPC();
            otherWindowUp = inspect.Shown || inventory.Shown;
        }
    }

    private bool GetIfInteractable(bool[] actionsAvailable)
    {
        for (int i = 0; i < actionsAvailable.Length; i++)
        {
            if (actionsAvailable[i])
            {
                return true;
            }
        }
        return false;
    }

    private void HandleWheelSelection(int selection)
    {

        //Debug.Log(selection);

        switch (selection)
        {
            case 0:
                string targetName = targetController.Inspect();
                if(targetName.IndexOf('*') == -1)
                    inspect.Show(targetName);
                else
                {
                    int star = targetName.IndexOf('*');
                    string thename = targetName.Substring(0, star);
                    string descrip = targetName.Substring(star+1);
                    if(descrip.IndexOf('*') == -1)
                        inspect.Show(thename, descrip);
                    else
                    {
                        star = descrip.IndexOf('*');
                        string type = descrip.Substring(0, star);
                        descrip = descrip.Substring(star + 1);
                        inspect.Show(thename, type, descrip);

                    }
                }
                otherWindowUp = true;
                break;
            case 1:
                if (PlayerController.instance.GetActiveCharacter() != PlayerController.PlayerCharacter.Mimbi)
                {
//                    Debug.Log("Selected"); 
                    targetController.Talk();
                    followCamera.LockOff();
                }
                else
                    feedback.ShowText("Sorry " + targetController.Inspect() + "  can't understand Mimbi.");
                break;
            case 2:
                if (PlayerController.instance.GetActiveCharacter() == PlayerController.PlayerCharacter.Mimbi)
                {
                    targetController.Distract(PlayerController.instance.GetActivePlayerObject());
                    StartCoroutine(DistractionEnd(distractTime, targetController));
                    PlayerController.instance.GetComponent<PlayerController>().StartMimbiDistraction();
                    //PlayerController.instance.GetComponent<PlayerAnimatorController>().EnableDistraction();
                    interaction.NotAllowed = true;
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
        HideWheel();
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
        if (Input.GetButtonDown("Interact"))
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
        PlayerController.instance.RestoreSheathState(); 
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

    private void ApplyOutlineToNPC(GameObject npc)
    {
        outlined = true;
        foreach (Renderer renderer in npc.GetComponentsInChildren<Renderer>())
        {
            //Debug.Log(renderer.gameObject.layer);
            if (renderer.gameObject.layer != 13)
            {
                renderer.gameObject.AddComponent<Outline>();
            }
        }
    }
    private void RemoveOutlineToNPC(GameObject npc)
    {
        foreach (Renderer renderer in npc.GetComponentsInChildren<Renderer>())
        {
            if (renderer.gameObject.layer != 13)
            {
                Destroy(renderer.gameObject.GetComponent<Outline>());
            }
        }
        outlined = false;
    }
}
