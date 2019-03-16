using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dialogue {
    [NodeTint("#CCFFCC")]
    public class Chat : DialogueBaseNode {
        [Input] public Connection input;
        [Output] public Connection output;
        [TextArea] public string text;
        public List<Answer> answers = new List<Answer>();

        [System.Serializable] public class Answer {
            public string text;
            public AudioClip voiceClip;
        }

        public void AnswerQuestion(int index) {
            NodePort port = null;
            if (answers.Count == 0) { //if no answer options, just go to next default node
                port = GetOutputPort("output");
            } else { //otherwise, get output port for answer
                if (answers.Count <= index) return;
                port = GetOutputPort("answers " + index);
            }

            if (port == null) return; 
            for (int i = 0; i < port.ConnectionCount; i++) {
                NodePort connection = port.GetConnection(i);
                (connection.node as DialogueBaseNode).Trigger();
            } 
        }

        public override void Trigger() {
//            Debug.Log("We got a new dialogue base");
            (graph as DialogueGraph).current = this;
        }
    }
}