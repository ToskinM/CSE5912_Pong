using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataSavingManager : MonoBehaviour
{
    public static DataSavingManager dataSavingManager;

    private IPlayerController anaiController;
    private IPlayerController mimbiController;



    void Start()
    {
        if (dataSavingManager == null)
        {
            DontDestroyOnLoad(gameObject);
            dataSavingManager = this;
            GetReferences();
        }
        else if (dataSavingManager != null)
        {
            Destroy(gameObject);
        }
    }

    private void GetReferences()
    {
        anaiController = GameObject.Find("Anai").GetComponent<AnaiController>();
        mimbiController = GameObject.Find("Mimbi").GetComponent<MimbiController>();
    }

    public void SaveGame()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        SavePlayerInfo(binaryFormatter);
    }

    private void SavePlayerInfo(BinaryFormatter binaryFormatter)
    {
        FileStream file = File.Open(Application.persistentDataPath + "/PlayerInfo.dat", FileMode.Open);

    }
}
