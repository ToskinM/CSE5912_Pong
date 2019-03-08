using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FeedbackText : MonoBehaviour
{
    TextMeshProUGUI text;
    //MeshRenderer textMeshRenderer;

    private enum State {displayed, fadingIn, fadingOut, gone}
    State state = State.gone;
    const float inc = 0.05f;

    const int timerMax = 80;
    int timerCount = 0; 

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        //textMeshRenderer = textMeshRenderer.GetComponent<MeshRenderer>();
        Color color = text.color; 
        text.color = new Color(color.r, color.g, color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.fadingIn:
                Color color = text.color;
                text.color = new Color(color.r, color.g, color.b, color.a + inc);
                Debug.Log("show fading in");
                if (text.color.a >= 1)
                {
                    text.color = new Color(color.r, color.g, color.b, 1);
                    state = State.displayed;
                }
                break;
            case State.fadingOut:
                Debug.Log("show fading out");
                color = text.color;
                text.color = new Color(color.r, color.g, color.b, color.a - inc);
                Debug.Log(color.a);
                if (text.color.a <= 0)
                {
                    text.color = new Color(color.r, color.g, color.b, 0);
                    state = State.gone;
                }
                break;
            case State.displayed:
                Debug.Log("show the text!");
                color = text.color;
                if (text.color.a <= 1)
                {
                    text.color = new Color(color.r, color.g, color.b, 1);
                    state = State.displayed;
                }
                timerCount++;
                if (timerCount >= timerMax)
                {
                    state = State.fadingOut;
                    timerCount = 0;
                }
                break;
            case State.gone:
                color = text.color;
                text.color = new Color(color.r, color.g, color.b, 0);
                break;
        }

    }

    public void ShowText(string t)
    {
        timerCount = 0;
        text.text = t;
        if (state == State.gone || state == State.fadingOut)
        {
            state = State.fadingIn;
        }
        else
        {
            state = State.displayed;
        }
    }

    public void KillText()
    {
        state = State.fadingOut;
    }


}
