using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Camera playerCamera;

    //Shooting settings
    public bool isShooting; 
    public bool readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 0.2f;

    //Burst settings
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    public int currentBurst;

    //Spread settings
    public float spreadIntensity;

    //bullet settings
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 100;
    public float bulletPrefabLifeTime = 3f;

    //Shooting Modes
    public enum ShootingMode { 
    
        Single,
        Burst,
        Auto

    }
    public ShootingMode currentShootingMode;

    private void Awake() {
        readyToShoot = true;
        currentBurst = bulletsPerBurst;
    }

    // Update is called once per frame
    void Update() {

        //This checks if current shooting mode is auto and makes it so you need to hold down the fire button
        if (currentShootingMode == ShootingMode.Auto) {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        //This checks if the shooting mode is single or burst and makes the fire button single press
        else if (currentShootingMode == ShootingMode.Single ||
            currentShootingMode == ShootingMode.Burst) {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        //controls fire mode actually shooting
        if (readyToShoot && isShooting) {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon() {

        //resets shooting state so problems don't occur in the loop
        readyToShoot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //This spawns the bullet at bulletSpawn location with the default rotation transform position
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //points the bullet to face the shooting direction
        bullet.transform.forward = shootingDirection;

        //This applies force to the bullet object to make it appear as if its firing
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity, ForceMode.Impulse);

        //calls method to destroy bullet after time and sets parameters of DestroyBulletAfterTime
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        //checks if shooting is done
        if (allowReset) {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //Burst Mode
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1) {//one shot has already happened by this point
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot() {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread() {

        //Ray shoots from the middle of the screen to check where we are aiming os bullet can go there
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit)) {

            //when the ray hits something
            targetPoint = hit.point;
        }
        else {
            //when the ray misses and goes into the air
            targetPoint = ray.GetPoint(100);
        }
        //find distance from bullet spawn to target
        Vector3 direction = targetPoint - bulletSpawn.position;

        //calculates spread using distance from spawn to target and random numbers for coordinates
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //returns the shooting direction and the spread
        return direction + new Vector3(x, y, 0);
    }

    //Despawns bullet after some time to improve performance
    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay) {

        yield return new WaitForSeconds(delay);
        Destroy(bullet);

    }
}
