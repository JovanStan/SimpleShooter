using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float waitAfterDying = 2f;


    public GameObject boss;

    public GameObject portal;

    public bool isBossActive = false;



    public bool levelEnding;

    private void Awake()
    {
   
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        portal.SetActive(false);
        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }


        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 &&  isBossActive == false)
        {
            boss.SetActive(true);
            isBossActive = true;
        }

        if (boss == null)
        {
            portal.SetActive(true);
        }
    }



    public void PlayerDied()
    {
        StartCoroutine(PlayerDiedCo()); 
    }

    // cekamo neko vreme pre nego se respawn-amo ponovo
    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseUnpause()
    {
        if(UIController.instance.pauseScreen.activeInHierarchy)
        {
            UIController.instance.pauseScreen.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;

            PlayerController.instance.footStepFast.Play();
            PlayerController.instance.footStepSlow.Play();

        } else
        {
            UIController.instance.pauseScreen.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // zaustavimo vreme kad pretisnemo escape
            Time.timeScale = 0f;

            PlayerController.instance.footStepFast.Stop();
            PlayerController.instance.footStepSlow.Stop();
        }
    }
}
