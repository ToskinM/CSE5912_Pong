using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI MimbiGameOverText;
    public TextMeshProUGUI AnaiGameOverText;
    private void Start()
    {
        canvasGroup.alpha = 0f;
        StartCoroutine(Fade(1f));
        if (PlayerController.instance.GetActiveCharacter() == PlayerController.PlayerCharacter.Mimbi)
        {
            MimbiGameOverText.gameObject.SetActive(true);
            AnaiGameOverText.gameObject.SetActive(false);
        }
        else
        {
            MimbiGameOverText.gameObject.SetActive(false);
            AnaiGameOverText.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        GameStateController.current.ForceMouseUnlock();
    }

    public void OnContinueButtonClick()
    {
        SceneController.current.FadeAndLoadScene(Constants.SCENE_ANAIHOUSE);
    }
    public void OnMainMenuButtonClick()
    {
        SceneController.current.FadeAndLoadSceneNoLS(Constants.SCENE_MAINMENU);
    }

    private IEnumerator Fade(float finalAlpha, float fadeDuration = 4f)
    {
        canvasGroup.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
    }
}
