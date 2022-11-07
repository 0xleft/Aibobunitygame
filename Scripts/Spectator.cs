using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{   
    public float speed = 10f;
    public float sensitivity = 100f;

    private void Awake() {
        //lock the cursor in the middle of the screen and hide it.
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {   
        
        //player rotation y axes convert to rad
        float angleR = transform.localEulerAngles.y * Mathf.Deg2Rad;
        //move players head acording to the mouse movement
        transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity, Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity, 0);
        //move player acording to its angle
        transform.position += new Vector3(Mathf.Sin(angleR) * speed * Input.GetAxis("Vertical"), 0, Mathf.Cos(angleR) * speed * Input.GetAxis("Vertical")) * Time.deltaTime;
        
    }
}
