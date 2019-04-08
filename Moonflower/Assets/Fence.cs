using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fence : MonoBehaviour
{
    public FeedbackText Feedback;
    // Start is called before the first frame update
    void Start()
    {
        Feedback = GameObject.Find("FeedbackText").GetComponent<FeedbackText>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == PlayerController.instance.GetActivePlayerObject())
        Feedback.ShowText("It is dangerous to go that far");
    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
