using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAvalibleArea : MonoBehaviour
{
    public GameObject PlayerAnai;
    public GameObject PlayerMimbi;
    private GameObject CurrentPlayer;
    public int hearableRadius;

    // Start is called before the first frame update
    void Start()
    {
        hearableRadius = 10;
    }
    private void CheckCurrentPlayer()
    {
        if (PlayerAnai.GetComponent<AnaiController>().Playing == true)
            CurrentPlayer = PlayerAnai;
        else
            CurrentPlayer = PlayerMimbi;
    }
    public void HearableArea(AudioSource audioSource, float volume)
    {
        CheckCurrentPlayer();
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
