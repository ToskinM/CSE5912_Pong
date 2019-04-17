using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    public static GameStateController current;

    public bool Paused;
    public bool DebugModeOn;
    public bool DebugViewOn;
    public bool cameraAvailable;
    public Dictionary<string, DialogueTrigger> NPCDialogues;
    public Dictionary<string, string> NPCDialogueLabels;

    private FollowCamera camControl;

    public delegate void Pause(bool isPaused);
    public static event Pause OnPaused;

    public delegate void FreezePlayer(bool freeze);
    public static event FreezePlayer OnFreezePlayer;

    public delegate void UpdateDebugView(bool debugViewOn);
    public static event UpdateDebugView OnDebugViewToggle;

    private int menuLayers = 0;
    private int pauseLayers = 0;

    int time = 8;
    public bool Passed = false;

    void Awake()
    {
        NPCDialogues = new Dictionary<string, DialogueTrigger>();
        NPCDialogueLabels = new Dictionary<string, string>();
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }

        Paused = false;
        DebugModeOn = false;

    }

    private void Start()
    {
        // Lock Mouse on game start
        if (SceneManager.GetActiveScene().name != Constants.SCENE_MAINMENU)
        {
            SetMouseLock(true);
        }

        if (LevelManager.current && LevelManager.current.mainCamera)
        {
            cameraAvailable = true;
            camControl = LevelManager.current.mainCamera;
        }
    }

    void Update()
    {
        if (cameraAvailable)
        {
            camControl = LevelManager.current.mainCamera;
        }
    }

    public bool SaveTime()
    {
        GameObject sky = GameObject.Find("Sky");
        if (sky != null)
        {

            time = sky.GetComponent<SkyColors>().GetTime();
//            Debug.Log("save time " + time);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RestoreTime(SkyColors sky)
    {
        //GameObject sky = GameObject.Find("Sky");
        sky.SetTime(time);
//        Debug.Log("set time " + time);
        return true;
    }


    public void SaveNPCDialogues(string name, string label, DialogueTrigger dia)
    {
        if (NPCDialogues.ContainsKey(name))
        {
            NPCDialogues.Remove(name);
        }
        NPCDialogues.Add(name, dia);
        if (NPCDialogueLabels.ContainsKey(name))
        {
            NPCDialogueLabels.Remove(name);
        }
       
        NPCDialogueLabels.Add(name, label); 
    }

    public DialogueTrigger GetNPCDialogue(string charName)
    {
        if (NPCDialogues.ContainsKey(charName))
        {
 //           Debug.Log("get dialogue for " + charName);
            return NPCDialogues[charName];
        }
        else
        {
            return null;
        }
    }

    public string GetNPCDiaLabel(string charName)
    {
        if (NPCDialogues.ContainsKey(charName))
        {
            //           Debug.Log("get dialogue for " + charName);
            return NPCDialogueLabels[charName];
        }
        else
        {
            return "";
        }
    }


    public void ToggleDebugMode()
    {
        DebugModeOn = !DebugModeOn;

        //CameraController camControl = MainCamera.GetComponent<CameraController>();
        if (cameraAvailable)
        {
            camControl = LevelManager.current.mainCamera;
            camControl.SetFreeRoam(DebugModeOn);
            SetPlayerMovement(!DebugModeOn);
        }
    }

    public void ToggleDebugView()
    {
        DebugViewOn = !DebugViewOn;

        OnDebugViewToggle?.Invoke(DebugViewOn);
    }

    public void SetMouseLock(bool doLock)
    {
        if (doLock)   // Lock
        {
            menuLayers = (int)Mathf.Clamp(menuLayers - 1, 0, float.PositiveInfinity);
            if (menuLayers <= 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else          // Unlock
        {
            menuLayers++;
            if (menuLayers > 0)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void ForceMouseUnlock()
    {
        menuLayers = 0;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TogglePause()
    {
        if (Paused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
        
        OnPaused?.Invoke(Paused);

        if (camControl.Frozen)
            UnfreezeCamera();
        else
            FreezeCamera(); 
    }

    public void PauseGame()
    {
        pauseLayers++;
        if (pauseLayers > 0)
        {
            Paused = true;
            OnPaused?.Invoke(Paused);
            if (camControl)
            {
                camControl.Frozen = true;
            }
            //OnPaused?.Invoke(Paused);
            SetMouseLock(false);
            Time.timeScale = 0;

        }
    }
    public void UnpauseGame()
    {
        pauseLayers = (int)Mathf.Clamp(pauseLayers - 1, 0, float.PositiveInfinity);
        if (pauseLayers <= 0)
        {
            Paused = false;
            if (camControl)
            {
                camControl.Frozen = false;
            }
            //OnPaused?.Invoke(Paused);
            SetMouseLock(true);
            Time.timeScale = 1;
        }
    }
    public void ForceUnpause()
    {
        pauseLayers = 0;
        Paused = false;
        if (camControl)
            camControl.Frozen = false;
        //OnPaused?.Invoke(Paused);
        SetMouseLock(true);
        Time.timeScale = 1;
    }

    public void FreezeCamera()
    {
       camControl.Frozen = true; 
    }

    public void UnfreezeCamera()
    {
        camControl.Frozen = false;
    }

    private void SetPlayerMovement(bool enabled)
    {
        PlayerMovementController playerMovement = PlayerController.instance.gameObject.GetComponent<PlayerMovementController>();
        playerMovement.enabled = enabled;
    }

    public void SetPlayerFrozen(bool frozen)
    {
        OnFreezePlayer?.Invoke(frozen);
    }

    public void PlayerDeath()
    {
        StartCoroutine(PlayerDeathSequence());
    }

    private IEnumerator PlayerDeathSequence()
    {
        // Wait for player death animation
        yield return new WaitForSeconds(3f);

        // Fade out to black
        //yield return StartCoroutine(SceneController.current.FadeToBlack(3f));

        // Display game over menu
        //PauseGame();
        //GameObject gameOverScene = Instantiate((GameObject)Resources.Load("Menu/GameOver Canvas"));

        SceneController.current.FadeAndLoadSceneGameOver(Constants.SCENE_GAMEOVER);
        //SceneController.current.FadeOutToBlack(5f);
    }
}
