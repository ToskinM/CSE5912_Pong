using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public enum PlayerCharacter { Anai, Mimbi };
    private PlayerCharacter activeCharacter;

    public GameObject TalkingPartner { get; set; }
    public GameObject AnaiObject;
    public GameObject MimbiObject;

    public PlayerMovementController ActivePlayerMovementControls;
    public PlayerCombatController ActivePlayerCombatControls;
    public FieldOfView ActivePlayerInteractionFOV;
    PlayerInventory ActivePlayerInventory;
    PlayerAnimatorController ActivePlayerAnimator;

    GameObject ActivePlayerObject;

    public delegate void SwitchCharacter(PlayerCharacter activeChar);
    public static event SwitchCharacter OnCharacterSwitch;

    void Awake()
    {
        // Make this a singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

        ActivePlayerMovementControls = GetComponent<PlayerMovementController>();
        ActivePlayerAnimator = GetComponent<PlayerAnimatorController>();
        ActivePlayerCombatControls = GetComponent<PlayerCombatController>();
        ActivePlayerInventory = GetComponent<PlayerInventory>();

        // Set up reference in Awake so that other scripts can reference it during Start()
        ActivePlayerObject = AnaiObject;

        ActivePlayerInteractionFOV = ActivePlayerObject.GetComponent<FieldOfView>();
        ActivePlayerInteractionFOV.enabled = true;

        LevelManager.current.player = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        activeCharacter = PlayerCharacter.Anai;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayerSwitchInput();
        DetectSummonCompanionInput();

        UpdateCompanionCharacter();
    }

    private void DetectPlayerSwitchInput()
    {
        if (Input.GetButtonDown("Switch"))
        {
            SwitchActiveCharacter();
        }
    }

    private void DetectSummonCompanionInput()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SummonMimbi();
        }
    }

    private void SwitchActiveCharacter()
    {
        if (ActivePlayerObject == AnaiObject)
        {
            SwitchToMimbi();
        }
        else if (ActivePlayerObject == MimbiObject) 
        {
            SwitchToAnai();
        }

        LevelManager.current.currentPlayer = ActivePlayerObject;

        // Update active character in control scripts
        OnCharacterSwitch?.Invoke(activeCharacter);
    }

    private void SwitchToAnai()
    {
        activeCharacter = PlayerCharacter.Anai;
        ActivePlayerObject = AnaiObject;

        MimbiObject.GetComponent<FieldOfView>().enabled = false;
        ActivePlayerInteractionFOV = AnaiObject.GetComponent<FieldOfView>();
        ActivePlayerInteractionFOV.enabled = true;

        AnaiObject.tag = "Player";
        MimbiObject.tag = "Companion";

        // Disable Anai's AI, enable Mimbi's AI
        MimbiObject.GetComponent<NavMeshAgent>().enabled = true;
        AnaiObject.GetComponent<NavMeshAgent>().enabled = false;

        // Disable Mimbi 's Collision Listener and enable Anai's
        AnaiObject.GetComponent<PlayerColliderListener>().enabled = true;
        MimbiObject.GetComponent<PlayerColliderListener>().enabled = false;
    }

    private void SwitchToMimbi()
    {
        activeCharacter = PlayerCharacter.Mimbi;
        ActivePlayerObject = MimbiObject;

        AnaiObject.GetComponent<FieldOfView>().enabled = false;
        ActivePlayerInteractionFOV = MimbiObject.GetComponent<FieldOfView>();
        ActivePlayerInteractionFOV.enabled = true;

        MimbiObject.tag = "Player";
        AnaiObject.tag = "Companion";

        // Disable Mimbi's AI, enable Anai's AI
        AnaiObject.GetComponent<NavMeshAgent>().enabled = true;
        MimbiObject.GetComponent<NavMeshAgent>().enabled = false;

        // Disable Anai's Collision Listener and enable Mimbi's
        MimbiObject.GetComponent<PlayerColliderListener>().enabled = true;
        AnaiObject.GetComponent<PlayerColliderListener>().enabled = false;
    }

    public void SummonMimbi()
    {
        ActivePlayerMovementControls.MimbiPassiveController.RunToPlayer();
    }

    // Disable updates when gaame is paused
    void HandlePauseEvent(bool isPaused)
    {
        enabled = !isPaused;
    }

    // Disable player controls
    void HandleFreezeEvent(bool frozen)
    {
        ActivePlayerMovementControls.Action = PlayerMovementController.Actions.Chilling;
        enabled = !frozen;
    }

    public PlayerCharacter GetActiveCharacter()
    {
        return activeCharacter;
    }

    public bool AnaiIsActive()
    {
        return activeCharacter == PlayerCharacter.Anai; 
    }

    public GameObject GetActivePlayerObject()
    {
        return ActivePlayerObject;
    }

    public GameObject GetCompanionObject()
    {
        if (activeCharacter == PlayerCharacter.Anai)
        {
            return MimbiObject;
        }
        else
        {
            return AnaiObject;
        }
    }

    void UpdateCompanionCharacter()
    {
        ActivePlayerMovementControls.UpdateCompanionMovement(activeCharacter);
        // ActivePlayerCombatControls.UpdateCompanionCombat(activeCharacter);
        ActivePlayerAnimator.UpdateCompanionAnimation(activeCharacter);
    }
}
