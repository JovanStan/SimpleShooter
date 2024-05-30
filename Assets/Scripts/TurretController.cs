using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject bullet;

    public float rangeToTargetPlayer, timeBeweenShots = .5f;
    private float shotCounter;

    public Transform gun, firepoint;

    public float rotateSpeed = 45f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = timeBeweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        // kada da nas turret puca
        if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToTargetPlayer)
        {
            gun.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.3f, 0f));

            shotCounter -= Time.deltaTime;

            if(shotCounter <= 0)
            {
                Instantiate(bullet, firepoint.position, firepoint.rotation);
                shotCounter = timeBeweenShots;
            }
        } else
        {
            shotCounter = timeBeweenShots;

            // rotacija 
            gun.rotation = Quaternion.Lerp(gun.rotation, Quaternion.Euler(0f, gun.rotation.eulerAngles.y + 10f, 0f), rotateSpeed * Time.deltaTime);
        }
    }
}
