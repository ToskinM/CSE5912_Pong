using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimbiMovement : MonoBehaviour
{
    private float moveSpeed;
    public GameObject targetObject;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 2f;
    public float rotateSpeed = 15f;
    public bool playing;
    // Start is called before the first frame update
    void Start()
    {
        playing = false;
        moveSpeed = 5f;
        //targetObject = GameObject.FindGameObjectWithTag("Pyayer");
        //if (targetObject==null)
        //{
        //    Debug.Log("can't find anai");
        //}
    }
    void KeyInput()
    {
        // Match camera y rotation if moving
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        //{
        //    Quaternion rotation = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
        //}

        if (Input.GetKey(KeyCode.W))
        {
            //need change when prefab is changed
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //need change when prefab is changed
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * 10 * Time.deltaTime * rotateSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(-Vector3.up * 10 * Time.deltaTime * rotateSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Switch"))
        { 
            playing = !playing;
        }
        if (playing)
        {
            KeyInput();
            //Debug.Log("I AM USING MIMBI");

        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(targetObject.transform.position.x + 1, targetObject.transform.position.y - 0.5f, targetObject.transform.position.z - 1), ref velocity, smoothTime);
        }
}
}
