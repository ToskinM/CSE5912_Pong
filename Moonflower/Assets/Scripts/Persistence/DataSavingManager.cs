using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataSavingManager : MonoBehaviour
{
    public static DataSavingManager current;

    AnaiData playerData;

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
        anai = GameObject.Find("Anai");
        mimbi = GameObject.Find("Mimbi");
        playerData = null;
    }

    public void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        SavePlayerInfo(binaryFormatter);
    }
    public void LoadGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        LoadPlayerInfo(binaryFormatter);
    }

    private void SavePlayerInfo(BinaryFormatter binaryFormatter)
    {
        //    FileStream file = File.Open(anaiDataFilePath, FileMode.Open);

        //    AnaiData playerData = new AnaiData(anai);

        //    binaryFormatter.Serialize(file, playerData);

        //    file.Close();

        playerData = new AnaiData(anai);
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

        if (playerData != null)
        {
            playerData.Load(anai);

        }

    }


}
