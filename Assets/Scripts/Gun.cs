using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun instance;

    public GameObject bullet;

    public bool canAutoFire;

    public float fireRate;
    // da ne mozemo unutar unity-a da menjamo vrednost iako je public
    [HideInInspector]
    public float fireCounter;

    public int currentAmmo, pickupAmount;

    public Transform firepoint;

    public float zoomAmount;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    public void GetAmmo()
    {
        currentAmmo += pickupAmount;

        UIController.instance.ammoText.text = "AMMO: " + currentAmmo;
    }
}
