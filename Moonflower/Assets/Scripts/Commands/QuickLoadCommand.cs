
using UnityEngine;

public class QuickLoadCommand : ICommand
{
    public void Execute()
    {
        DataSavingManager.current.LoadGame();
        Debug.Log("QuickLoading...");
    }
    public void Unexecute()
    {
        //not a thing
    }
}