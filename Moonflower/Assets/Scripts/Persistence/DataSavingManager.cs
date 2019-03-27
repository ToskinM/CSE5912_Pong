using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSavingManager : MonoBehaviour
{
    public static DataSavingManager current;

    public AnaiData anaiData = null;
    public MimbiData mimbiData = null;
    public CameraData cameraData = null;
    public NPCData npcData = null;

    public GameObject anai;
    public GameObject mimbi;

    private string anaiDataFilePath;

    void Start()
    {
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
            GetReferences();
            npcData = new NPCData(); 
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }
    }

    private void GetReferences()
    {
        anaiDataFilePath = Application.persistentDataPath + "/AnaiInfo.dat";
        anai = LevelManager.current.anai;
        mimbi = LevelManager.current.mimbi;
    }

    public void SaveNPCDialogues(string name, DialogueTrigger dia)
    {
        if(npcData.NPCDialogues.ContainsKey(name))
        {
            npcData.NPCDialogues.Remove(name); 
        }
        npcData.NPCDialogues.Add(name, dia);
    }

    public DialogueTrigger GetNPCDialogue(string charName)
    {
        if (npcData.NPCDialogues.ContainsKey(charName))
            return npcData.NPCDialogues[charName];
        else
            return null;
    }

    public void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        SavePlayerInfo(binaryFormatter);
        cameraData = new CameraData(LevelManager.current.mainCamera);
        npcData = new NPCData();
    }
    public void LoadGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //LoadPlayerInfo();
        //cameraData?.Load();
        //npcData?.Load();
    }

    private void SavePlayerInfo(BinaryFormatter binaryFormatter)
    {
        //    FileStream file = File.Open(anaiDataFilePath, FileMode.Open);

        //    AnaiData playerData = new AnaiData(anai);

        //    binaryFormatter.Serialize(file, playerData);

        //    file.Close();

        anaiData = new AnaiData(anai);
        mimbiData = new MimbiData(mimbi);
    }

    public void LoadPlayerInfo()
    {
        //if (File.Exists(anaiDataFilePath))
        //{
        //    FileStream file = File.Open(anaiDataFilePath, FileMode.Open);
        //    AnaiData anaiData = (AnaiData)binaryFormatter.Deserialize(file);
        //    file.Close();

        //    anai.transform.SetPositionAndRotation(anaiData.transform.position, anaiData.transform.rotation);
        //}

        anaiData?.Load(LevelManager.current.anai);
        mimbiData?.Load(LevelManager.current.mimbi);

    }


}
