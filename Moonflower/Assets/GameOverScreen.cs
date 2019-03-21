using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void OnContinueButtonClick()
    {
        SceneController.current.FadeAndLoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnMainMenuButtonClick()
    {
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_MAINMENU);
    }
}
