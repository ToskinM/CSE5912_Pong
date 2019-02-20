using UnityEngine;

public class QuickSaveCommand : ICommand
{
    public void Execute()
    {
        DataSavingManager.current.SaveGame();
        Debug.Log("QuickSaving...");
    }
    public void Unexecute()
    {
        //not a thing
    }
}
