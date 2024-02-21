using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed = 12f;
    public float gravityMultiplier;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private CharacterController controller;


    // Start is called before the first frame update
    void Start() {

        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update() {

        //Ground position check relative to self
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset the default velocity
        if (isGrounded && velocity.y < 0) {

            velocity.y = -2f;
       
        }

        //Gets inputs 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
      

        //Does math to create the vector for moving the player
        Vector3 move = transform.right * x + transform.forward * z; //(right = red axis, forward = blue axis)

        //Move the player
        controller.Move(move * speed * Time.deltaTime);

        //Check if the player can jump
        if (Input.GetButtonDown("Jump") && isGrounded) {

            //Does math so player can jump
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        }

        //Applies gravity for falling down
        velocity.y += gravity * Time.deltaTime;

        //Executing the jump
        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true) {

            isMoving = true;
     
        }
        else {

            isMoving = false;

        }


        lastPosition = gameObject.transform.position;

    }
}
