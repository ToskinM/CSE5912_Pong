using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using XNode;
namespace Dialogue
{
    [NodeTint("#FFFFAA")]
    public class Event : DialogueBaseNode
    {
        [Input] public Connection input;
        [Output] public Connection output;

        //public SerializableEvent[] trigger; // Could use UnityEvent here, but UnityEvent has a bug that prevents it from serializing correctly on custom EditorWindows. So i implemented my own.
        public UnityEvent[] trigger; 

        public override void Trigger()
        {
            for (int i = 0; i < trigger.Length; i++)
            {
                trigger[i].Invoke();
            }
            NodePort port = null;
            port = GetOutputPort("output");
            if (port == null) return;
            NodePort connection = port.GetConnection(0);
            (connection.node as DialogueBaseNode).Trigger();

        }
    }
}