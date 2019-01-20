using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupBehavior : MonoBehaviour
{
    public Button yes, no;
    ICommand cont; 
    void Start()
    {
        yes.onClick.AddListener(Quit);
        no.onClick.AddListener(Continue);

        cont = new ReturnMenuCommand();
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit(); // this doesn't affect the unity editor, only a built application
    }

    public void Continue()
    {
        cont.Execute();
    }

}
