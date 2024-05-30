using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12f;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    public Transform firePoint;

    public Gun activeGun;

    public List<Gun> allGuns = new List<Gun>();
    public int currentGun;


    public Transform adsPoint, gunHolder;
    private Vector3 gunStartPos;
    public float adsSpeed = 6f;

    public GameObject flash;

    public AudioSource footStepFast, footStepSlow;

    



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // sve ovo u start sluzi da na pocetnu igre imamo pistolj u ruci.
            activeGun.gameObject.SetActive(false);

            activeGun = allGuns[0];
            activeGun.gameObject.SetActive(true);

            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            firePoint.position = activeGun.firepoint.position;

             gunStartPos = gunHolder.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy && !GameManager.instance.levelEnding)
        {

            // skladistimo y brzinu
            float yStore = moveInput.y;


            // kretanje
            Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
            Vector3 horMove = transform.right * Input.GetAxis("Horizontal");

            moveInput = horMove + vertMove;
            moveInput.Normalize();

            // trcanje
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveInput = moveInput * runSpeed;
            }
            else
            {
                moveInput = moveInput * moveSpeed;
            }

            moveInput.y = yStore;

            // gravitacija
            moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

            if (charCon.isGrounded)
            {
                moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
            }

            canJump = Physics.OverlapSphere(groundCheckPoint.position, .25f, whatIsGround).Length > 0;


            // skok
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                moveInput.y = jumpPower;

                AudioManager.instance.PlaySFX(6);
            }

            // dodeljujemo karakteru(playeru) da moze da se krece
            charCon.Move(moveInput * Time.deltaTime);

            // rotacija kamere
            Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

            // da li zelimo invert kontrole
            if (invertX)
            {
                mouseInput.x = -mouseInput.x;
            }
            if (invertY)
            {
                mouseInput.y = -mouseInput.y;
            }

            // levo - desno
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

            // gore - dole
            camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

            flash.SetActive(false);


            // pucanje single shots
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0)
            {
                // metak prati image koji sam postavio kao nisan
                RaycastHit hit;
                if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
                {
                    if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                    {
                        firePoint.LookAt(hit.point);
                    }
                }
                else
                {
                    firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
                }

                FireShot();
            }

            //  repeating shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }

            // switch guns
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                activeGun.gameObject.SetActive(false);

                activeGun = allGuns[0];
                activeGun.gameObject.SetActive(true);

                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

                firePoint.position = activeGun.firepoint.position;
                // SwitchGun();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                activeGun.gameObject.SetActive(false);

                activeGun = allGuns[1];
                activeGun.gameObject.SetActive(true);

                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

                firePoint.position = activeGun.firepoint.position;
                // SwitchGun();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                activeGun.gameObject.SetActive(false);

                activeGun = allGuns[2];
                activeGun.gameObject.SetActive(true);

                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

                firePoint.position = activeGun.firepoint.position;
               
            }
        /*    if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                activeGun.gameObject.SetActive(false);

                activeGun = allGuns[3];
                activeGun.gameObject.SetActive(true);

                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

                firePoint.position = activeGun.firepoint.position;
                
            } */

            // zoom in
            if (Input.GetMouseButton(1))
            {
                CameraController.instance.ZoomIn(activeGun.zoomAmount);
                UIController.instance.slika.SetActive(true);
            } else
            {
                UIController.instance.slika.SetActive(false);
            } 


      /*      if(Input.GetMouseButton(1) && Gun.instance.isSniper == true)
            {
                UIController.instance.slika.SetActive(true);
            } else
            {
                UIController.instance.slika.SetActive(false);
            }*/


            // pomeramo mesto puske dok nisanimo
            if (Input.GetMouseButton(1))
            {
               
                gunHolder.position = Vector3.MoveTowards(gunHolder.position, adsPoint.position, adsSpeed * Time.deltaTime);
            }
            else
            {
                gunHolder.localPosition = Vector3.MoveTowards(gunHolder.localPosition, gunStartPos, adsSpeed * Time.deltaTime);
            }

            // zoom out
            if (Input.GetMouseButtonUp(1))
            {
                CameraController.instance.ZoomOut();
            }

            // animacija hodanja
            anim.SetFloat("moveSpeed", moveInput.magnitude);
            anim.SetBool("onGround", canJump);
        }
    }



    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            activeGun.currentAmmo--;

            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

            flash.SetActive(true);
        }
    }

  /*  public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;

        if(currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;
    } */
}
