using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class GameStateManager : MonoBehaviour, IGameState
{
    public bool Paused { get; set; }
    public TextMeshProUGUI AIScoreDisplay;
    public TextMeshProUGUI PlayerScoreDisplay;
    public Canvas GeneralCanvas;

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
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, Screen.height - 60, 150, 50), "View Video"))
        {

        }
    }

    public void TogglePause()
    {
        Paused = !Paused;
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
        playerScore++;
        PlayerScoreDisplay.text = "" + playerScore;
    }
    public void Lose()
    {
        AIscore++;
        AIScoreDisplay.text = "" + AIscore;
    }

    public Vector3 OutOfBounds(Vector3 v)
    {
        if(v.y > topwall.transform.position.y - offset)
        {
            return new Vector3(v.x, topwall.transform.position.y - offset, v.z); 
        }
        if(v.y < bottomwall.transform.position.y + offset)
        {
            return new Vector3(v.x, bottomwall.transform.position.y + offset, v.z);
        }
        return v; 
    }
}
