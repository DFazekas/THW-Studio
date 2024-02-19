using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour {

    public float mouseSensitivity = 100f;
    public float topClamp = -90f;
    public float bottomClamp = 90f;


    float xRotation = 0f;
    float yRotation = 0f;


    // Start is called before the first frame update
    void Start() {

        //Locks cursor in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;

    }


    // Update is called once per frame
    void Update() {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotation around x axis (looking up and down)
        xRotation -= mouseY;
        //Clamps the rotation, stops the camera from moving more than the specified amount
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Rotation around y axis (looking left and right)
        yRotation += mouseX;

        //Apply the transform or movement of the input to the object
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }

}
