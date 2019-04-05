using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public enum PlayerCharacter { Anai, Mimbi };
    private PlayerCharacter activeCharacter;


    public GameObject TalkingPartner;
    public GameObject AnaiObject;
    public GameObject MimbiObject;

    public PlayerMovementController ActivePlayerMovementControls;
    public PlayerCombatController ActivePlayerCombatControls;
    public FieldOfView ActivePlayerInteractionFOV;
    PlayerInventory ActivePlayerInventory;
    public PlayerAnimatorController ActivePlayerAnimator;
    public CharacterStats ActivePlayerStats; 

    GameObject ActivePlayerObject;

    private bool isDead = false;
    private bool canSwitchCharacters = false;

    public delegate void SwitchCharacter(PlayerCharacter activeChar);
    public static event SwitchCharacter OnCharacterSwitch;

    void Awake()
    {
        // Make this a singleton
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
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
        ActivePlayerStats = GetComponent<CharacterStats>();

        // Set up reference in Awake so that other scripts can reference it during Start()
        ActivePlayerObject = AnaiObject;

        ActivePlayerInteractionFOV = ActivePlayerObject.GetComponent<FieldOfView>();
        ActivePlayerInteractionFOV.enabled = true;
    }

    private void OnEnable()
    {
        if (ActivePlayerCombatControls != null)
        {
            ActivePlayerCombatControls.OnDeath += HandlePlayerDeath;
        }
        else
        {
            Debug.Log("Active Player Combat Control is NULL");
        }
    }
    private void OnDisable()
    {
        if(ActivePlayerCombatControls != null )
            ActivePlayerCombatControls.OnDeath -= HandlePlayerDeath;
    }

    void Start()
    {
        activeCharacter = PlayerCharacter.Anai;
    }

    void Update()
    {
        if (!isDead)
        {
            DetectPlayerSwitchInput();
            DetectSummonCompanionInput();
        }

        UpdateCompanionCharacter();
    }

    private void DetectPlayerSwitchInput()
    {
        //Debug.Log(ActivePlayerMovementControls.Action);
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

    public void Revive()
    {
        ActivePlayerCombatControls.SetWeaponSheathed(true); 
        ActivePlayerAnimator.Reset();
        ActivePlayerMovementControls.Stunned = false;
        enabled = true;
        SwitchToAnai();

    }

    private void SwitchActiveCharacter()
    {
        if (!canSwitchCharacters || !GetCompanionObject().activeInHierarchy || TalkingPartner != null) return;

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
        AnaiObject.layer = 10;
        MimbiObject.tag = "Companion";
        MimbiObject.layer = 14;

        // Disable Anai's AI, enable Mimbi's AI
        //while(ActivePlayerMovementControls.MimbiPassiveController.Action ==Actions.Distracting)
        MimbiObject.GetComponent<NavMeshAgent>().enabled = true;
        AnaiObject.GetComponent<NavMeshAgent>().enabled = false;

        // Disable Mimbi 's Collision Listener and enable Anai's
        AnaiObject.GetComponent<PlayerColliderListener>().enabled = true;
        MimbiObject.GetComponent<PlayerColliderListener>().enabled = false;

        ActivePlayerMovementControls.MimbiPassiveController.Action = Actions.Chilling;
    }

    private void SwitchToMimbi()
    {
        activeCharacter = PlayerCharacter.Mimbi;
        ActivePlayerObject = MimbiObject;

        AnaiObject.GetComponent<FieldOfView>().enabled = false;
        ActivePlayerInteractionFOV = MimbiObject.GetComponent<FieldOfView>();
        ActivePlayerInteractionFOV.enabled = true;

        MimbiObject.tag = "Player";
        MimbiObject.layer = 10;
        AnaiObject.tag = "Companion";
        AnaiObject.layer = 14;

        // Disable Mimbi's AI, enable Anai's AI
        AnaiObject.GetComponent<NavMeshAgent>().enabled = true;
        MimbiObject.GetComponent<NavMeshAgent>().enabled = false;

        // Disable Anai's Collision Listener and enable Mimbi's
        MimbiObject.GetComponent<PlayerColliderListener>().enabled = true;
        AnaiObject.GetComponent<PlayerColliderListener>().enabled = false;
    }

    public void PassOut()
    {
        ActivePlayerAnimator.TriggerDeath(); 
    }

    public void ChillCompanion()
    {
        GetCompanionObject().GetComponent<PlayerMovementController>().CompanionMovementController.Chill(); 
    }

    public void ResetCompanion()
    {
        GetCompanionObject().GetComponent<PlayerMovementController>().CompanionMovementController.Reset();

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
        if (!GetCompanionObject().activeInHierarchy) return;
        // Temp fix Don't make mimbi move when distracting NPC
        if(ActivePlayerMovementControls.MimbiPassiveController.Action == Actions.Distracting)
        {
            Debug.Log("STOP!");
            //ActivePlayerMovementControls.MimbiPassiveController.Action = Actions.Chilling;
            MimbiObject.GetComponent<NavMeshAgent>().enabled=false;
        }
        else if (activeCharacter == PlayerCharacter.Anai)
        {
            //ActivePlayerMovementControls.MimbiPassiveController.Action = Actions.Chilling;
            MimbiObject.GetComponent<NavMeshAgent>().enabled = true;
        }
        if(GetCompanionObject().activeSelf)
            ActivePlayerMovementControls.UpdateCompanionMovement(activeCharacter);
        // ActivePlayerCombatControls.UpdateCompanionCombat(activeCharacter);
        ActivePlayerAnimator.UpdateCompanionAnimation(activeCharacter);
    }

    private void HandlePlayerDeath()
    {
        isDead = true;
        GameStateController.current.PlayerDeath();
    }

    public void SpawnPlayerObjects()
    {
        GameObject spawner = GameObject.Find("Spawner");
        spawner.GetComponent<SpawnPoint>().Spawn();
    }

    public void StartMimbiDistraction()
    {
        ActivePlayerMovementControls.SetToDistract(PlayerCharacter.Mimbi);
        ActivePlayerMovementControls.MimbiPassiveController.Action = Actions.Distracting;
        ActivePlayerAnimator.EnableDistraction();
    }

    public void EndMimbiDistraction()
    {
        ActivePlayerMovementControls.EndDistract(PlayerCharacter.Mimbi);
        ActivePlayerMovementControls.MimbiPassiveController.Action = Actions.Chilling;
        ActivePlayerAnimator.DisableDistraction();
    }

    public void EnableSwitching()
    {
//        Debug.Log("can switch!");
        canSwitchCharacters = true;
    }

    public void DisableSwitching()
    {
//        Debug.Log("Anai only bitch"); 
        canSwitchCharacters = false;
    }

}
