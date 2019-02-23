using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataSavingManager : MonoBehaviour
{
    public static DataSavingManager current;

    AnaiData anaiData = null;
    MimbiData mimbiData = null;
    CameraData cameraData = null;
    NPCData npcData = null;

    private GameObject anai;
    private GameObject mimbi;

    private string anaiDataFilePath;

    void Start()
    {
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
            GetReferences();
        }
        else if (current != null)
        {
            Destroy(gameObject);
        }
    }

    private void GetReferences()
    {
        anaiDataFilePath = Application.persistentDataPath + "/AnaiInfo.dat";
        anai = SceneTracker.current.anai;
        mimbi = SceneTracker.current.mimbi;
    }

    public void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        SavePlayerInfo(binaryFormatter);
        cameraData = new CameraData(SceneTracker.current.camera);
        npcData = new NPCData();


    }
    public void LoadGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        LoadPlayerInfo(binaryFormatter);
        cameraData?.Load();
        npcData?.Load();

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

    private void LoadPlayerInfo(BinaryFormatter binaryFormatter)
    {
        //if (File.Exists(anaiDataFilePath))
        //{
        //    FileStream file = File.Open(anaiDataFilePath, FileMode.Open);
        //    AnaiData anaiData = (AnaiData)binaryFormatter.Deserialize(file);
        //    file.Close();

        //    anai.transform.SetPositionAndRotation(anaiData.transform.position, anaiData.transform.rotation);
        //}

        anaiData?.Load(anai);
        mimbiData?.Load(mimbi);

    }


}
