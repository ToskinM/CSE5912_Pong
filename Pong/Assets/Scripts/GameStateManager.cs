using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class GameStateManager : MonoBehaviour, IGameState
{
    public bool Paused { get; set; }
    public TextMeshProUGUI AIScoreDisplay;
    public TextMeshProUGUI PlayerScoreDisplay;

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

    public void Pause()
    {
        Paused = !Paused;
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
