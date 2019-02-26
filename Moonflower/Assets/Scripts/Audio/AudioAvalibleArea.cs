using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAvalibleArea : MonoBehaviour
{
    private GameObject CurrentPlayer;
    public int hearableRadius;

    // Start is called before the first frame update
    void Start()
    {
        GetCurrentPlayer();
    }
    private void GetCurrentPlayer()
    {
        CurrentPlayer = GameObject.Find("Player").GetComponent<CurrentPlayer>().GetCurrentPlayer();
    }
    public void HearableArea(AudioSource audioSource, float volume)
    {
        GetCurrentPlayer();
        Vector3 x = CurrentPlayer.transform.position;
        if (Vector3.Distance(audioSource.transform.position, CurrentPlayer.transform.position) <= hearableRadius)
        {
            audioSource.volume = volume;
        }
        else
        {
            audioSource.volume = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
