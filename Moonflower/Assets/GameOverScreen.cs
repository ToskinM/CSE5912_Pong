using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void Update()
    {
        GameStateController.current.ForceMouseUnlock();
    }

    public void OnContinueButtonClick()
    {
        SceneController.current.FadeAndLoadScene(Constants.SCENE_VILLAGE);
    }
    public void OnMainMenuButtonClick()
    {
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_MAINMENU);
    }
}
