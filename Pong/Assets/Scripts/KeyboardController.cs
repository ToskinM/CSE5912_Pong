using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    Dictionary<KeyCode, ICommand> keyMapping;

    // Start is called before the first frame update
    void Start()
    {
        keyMapping = new Dictionary<KeyCode, ICommand>
        {
            { KeyCode.Escape, new ReturnMenuCommand() },
            { KeyCode.P, new PauseCommand() },
            { KeyCode.ScrollLock, new ScreenCaptureCommand() }
        };
    }

    // Update is called once per frame
    void Update()
    {
        foreach (KeyValuePair<KeyCode, ICommand> keyPair in keyMapping)
        {
            if (Input.GetKeyDown(keyPair.Key))
            {
                keyPair.Value.Execute();
            }
        }
    }
}
