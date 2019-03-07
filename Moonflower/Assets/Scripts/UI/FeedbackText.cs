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
    const float inc = 0.01f;

    int timerMax = 30;
    int timerCount = 0; 

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        //textMeshRenderer = textMeshRenderer.GetComponent<MeshRenderer>();
        Color color = text.faceColor; 
        text.faceColor = new Color(color.r, color.g, color.b, 0);

    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.fadingIn:
                Color color = text.faceColor;
                //text.faceColor = new Color(color.r, color.g, color.b, color.a + inc);
                //if (text.faceColor.a >= 1)
                //{
                //    text.faceColor = new Color(color.r, color.g, color.b, 1);
                //    state = State.displayed;
                //}
                break;
            case State.fadingOut:
        
                color = text.faceColor;
                text.faceColor = new Color(color.r, color.g, color.b, color.a - inc);
                if (text.faceColor.a <= 0)
                {
                    text.faceColor = new Color(color.r, color.g, color.b, 0);
                    state = State.gone;
                }
                break;
            case State.displayed:
                timerCount++;
                if (timerCount >= timerMax)
                {
                    state = State.fadingOut;
                    timerCount = 0;
                }
                break;
        
        }

    }

    public void ShowText(string t)
    {
        text.text = t;
        state = State.fadingIn; 
    }

    public void KillText()
    {
        state = State.fadingOut;
    }


}
