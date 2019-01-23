using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour, IGameState
{
    public bool Paused { get; set; }
    public TextMeshProUGUI AIScoreDisplay;
    public TextMeshProUGUI PlayerScoreDisplay;
    public Canvas GeneralCanvas;
    public ICommand end;

    public SaveData pongSaveData;
    public GameObject ball;
    public GameObject playerPaddle;
    public GameObject cpuPaddle;

    public AudioClip LoseClip;
    public AudioClip WinClip;
    public AudioSource MusicSource;

    GameObject topwall;
    GameObject bottomwall;
    float offset = 1.4f;
    int AIscore = 0;
    int playerScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        topwall = GameObject.Find("TopWall");
        bottomwall = GameObject.Find("BottomWall");
        Paused = false;
        end = new ReturnMenuCommand();
    }

    public void TogglePause()
    {
        Paused = !Paused;
    }

    public void SaveState()
    {
        pongSaveData.Save(Constants.SAVE_BALL_POSITION, ball.transform.position);
        pongSaveData.Save(Constants.SAVE_BALL_VELOCITY, ball.GetComponent<MaxxBall1>().velocity);
        pongSaveData.Save(Constants.SAVE_PLAYER_POSITION, playerPaddle.transform.position);
        pongSaveData.Save(Constants.SAVE_CPU_POSITION, cpuPaddle.transform.position);
        pongSaveData.Save(Constants.SAVE_PLAYER_SCORE, playerScore);
        pongSaveData.Save(Constants.SAVE_CPU_SCORE, AIscore);
    }
    public void LoadState()
    {
        Vector3 fetchVector3 = new Vector3();

        if (pongSaveData.Load(Constants.SAVE_BALL_POSITION, ref fetchVector3))
        {
            ball.transform.position = fetchVector3;

            pongSaveData.Load(Constants.SAVE_BALL_VELOCITY, ref ball.GetComponent<MaxxBall1>().velocity);

            pongSaveData.Load(Constants.SAVE_PLAYER_POSITION, ref fetchVector3);
            playerPaddle.transform.position = fetchVector3;

            pongSaveData.Load(Constants.SAVE_CPU_POSITION, ref fetchVector3);
            cpuPaddle.transform.position = fetchVector3;

            pongSaveData.Load(Constants.SAVE_PLAYER_SCORE, ref playerScore);
            pongSaveData.Load(Constants.SAVE_CPU_SCORE, ref AIscore);
            UpdateScores();
        }
    }
    public void ResetGame()
    {
        SceneController sceneController = FindObjectOfType<SceneController>();
        if (sceneController != null)
        {
            sceneController.FadeAndLoadScene(Constants.SCENE_PONG);
        }
    }

    // Should probably separate UI stuff to another script

    public void TogglePauseText()
    {
        GameObject hotkeyUICanvas = GeneralCanvas.transform.GetChild(1).gameObject;
        GameObject pauseText = hotkeyUICanvas.transform.GetChild(0).gameObject;

        bool isActive = Paused ? true : false;
        pauseText.SetActive(isActive);
    }

    public void DisplayScreencapText()
    {
        GameObject hotkeyUICanvas = GeneralCanvas.transform.GetChild(1).gameObject;
        GameObject screencapText = hotkeyUICanvas.transform.GetChild(1).gameObject;
        if (screencapText.activeInHierarchy) return;

        screencapText.SetActive(true);
        screencapText.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0.0f, 1.5f, false); // Fade out text
        Invoke("ResetScreencapText", 1.5f);
    }

    private void ResetScreencapText()
    {
        GameObject hotkeyUICanvas = GeneralCanvas.transform.GetChild(1).gameObject;
        GameObject screencapText = hotkeyUICanvas.transform.GetChild(1).gameObject;
        if (!screencapText.activeInHierarchy) return;

        screencapText.GetComponent<TextMeshProUGUI>().alpha = 1; // Reset text transparency
        screencapText.SetActive(false); // Disable text
    }

    public void Win()
    {
        MusicSource.clip = WinClip;
        MusicSource.Play();
        playerScore++;
        UpdateScores();
        if (playerScore == 3)
        {
            end.Execute();
        }
    }
    public void Lose()
    {
        MusicSource.clip = LoseClip;
        MusicSource.Play();
        AIscore++;
        UpdateScores();
        if (AIscore == 3)
        {
            end.Execute();
        }
    }
    public void UpdateScores()
    {
        PlayerScoreDisplay.text = "" + playerScore;
        AIScoreDisplay.text = "" + AIscore;
    }

    public Vector3 OutOfBounds(Vector3 v)
    {
        if (v.y > topwall.transform.position.y - offset)
        {
            return new Vector3(v.x, topwall.transform.position.y - offset, v.z);
        }
        if (v.y < bottomwall.transform.position.y + offset)
        {
            return new Vector3(v.x, bottomwall.transform.position.y + offset, v.z);
        }
        return v;
    }
}
