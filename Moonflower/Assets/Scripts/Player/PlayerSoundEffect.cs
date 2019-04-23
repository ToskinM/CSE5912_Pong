using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSoundEffect : MonoBehaviour
{
    private AudioManager audioManager;
    private readonly string anai = "Anai";
    private readonly string mimbi = "Mimbi";


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAudioManager());
    }
    public IEnumerator GetAudioManager()
    {
        while (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            yield return null;     }
    }
    public void AnaiAttackSFX()
    {
        audioManager.Play(anai,"AnaiAttackSwing");
    }
    public void AnaiWalkingSFX()
    {
        string Walk1;
        string Walk2;
        if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE || SceneManager.GetActiveScene().name == Constants.SCENE_CAVEENTRANCE)
        {
            Walk1 = "AnaiWalking(1)";
            Walk2 = "AnaiWalking(2)";
        }
        else
        {
            Walk1 = "AnaiWalking(3)";
            Walk2 = "AnaiWalking(4)";
        }

        //audioManager.ResumeNormal(anai,Walk1);
        //audioManager.ResumeNormal(anai, Walk2);
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
                audioManager.PlayFootStep(anai, Walk1);
            else
                audioManager.PlayFootStep(anai, Walk2);
        }

    }
    public void AnaiSneakingSFX()
    {
        string Walk1;
        string Walk2;
        if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE || SceneManager.GetActiveScene().name == Constants.SCENE_CAVEENTRANCE)
        {
            Walk1 = "AnaiWalking(1)";
            Walk2 = "AnaiWalking(2)";
        }
        else
        {
            Walk1 = "AnaiWalking(3)";
            Walk2 = "AnaiWalking(4)";
        }
        int x = Random.Range(1, 3);
        if (x == 1)
            audioManager.PlaySneakFootStep(anai, Walk1);
        else
            audioManager.PlaySneakFootStep(anai, Walk2);
    }

    public void AnaiIntoLava()
    {
        audioManager.Play(anai, "IntoLava");
    }

    public void AnaiPunchSFX()
    {
        audioManager.Play(anai, "AnaiPunch");
    }

    public void AnaiKickSFX()
    {
        audioManager.Play(anai, "AnaiKick");
    }

    public void AnaiRunSFX()
    {
        string Walk1;
        string Walk2;
        if (SceneManager.GetActiveScene().name == Constants.SCENE_VILLAGE || SceneManager.GetActiveScene().name == Constants.SCENE_CAVEENTRANCE)
        {
            Walk1 = "AnaiRun(1)";
            Walk2 = "AnaiRun(2)";
        }
        else
        {
            Walk1 = "AnaiRun(3)";
            Walk2 = "AnaiRun(4)";
        }
        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
                audioManager.PlayFootStep(anai, Walk1);
            else
                audioManager.PlayFootStep(anai, Walk2);
        }

    }
    public void PlayerPickupSFX()
    {
        audioManager.Play("Player", "PlayerPickup");
    }

    public void MimbiAttackSFX()
    {
        audioManager.Play(mimbi,"MimbiAttack");
    }

    public void MimbiWalkingSFX()
    {
        audioManager.ResumeNormal(mimbi, "MimbiWalking(1)");
        audioManager.ResumeNormal(mimbi, "MimbiWalking(2)");
        int x = Random.Range(1, 3);
        if (x == 1)
            audioManager.PlayFootStep(mimbi, "MimbiWalking(1)");
        else
            audioManager.PlayFootStep(mimbi, "MimbiWalking(2)");
    }
    public void MimbiSneakingSFX()
    {
        int x = Random.Range(1, 3);
        if (x == 1)
            audioManager.PlaySneakFootStep(mimbi, "MimbiWalking(1)");
        else
            audioManager.PlaySneakFootStep(mimbi, "MimbiWalking(2)");
    }

    public void MimbiRuningSFX()
    {
        int x = Random.Range(1, 3);
        if (x == 1)
            audioManager.Play(mimbi, "MimbiRuning(1)");
        else
            audioManager.Play(mimbi, "MimbiRuning(2)");
    }

    public void MimbiGetHitSFX()
    {
        audioManager.PlaySneakFootStep(mimbi, "MimbiGetHit");
        Debug.Log("get hit");
    }

    public void AnaiMute()
    {
        audioManager.MuteCategoryVol(anai);
    }
    public void AnaiResume()
    {
        audioManager.ResumeCategoryVol(anai);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
