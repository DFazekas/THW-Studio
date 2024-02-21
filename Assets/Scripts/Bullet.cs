using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {

        //checks if bullet hits something with the "Target" tag
        if (collision.gameObject.CompareTag("Target")) {

            //When the bullet hits something shows "hit" on the screen
            print("hit " + collision.gameObject.name + " !");

            //immediately destroys the bullet when it hits something
            Destroy(gameObject);

        }

        //checks if bullet hits something with the "wall" tag
        if (collision.gameObject.CompareTag("Wall")) {

            //When the bullet hits something shows "hit" on the screen
            print("hit a wall");

            //immediately destroys the bullet when it hits something
            Destroy(gameObject);

        }

    }



}
