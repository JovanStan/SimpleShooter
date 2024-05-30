using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreen : MonoBehaviour
{
    public string mainMenuScene;

    public float timeBetweenShowing = 1f;

    public GameObject secondText, returnButton, firstText;

    public Image blackScreen;
    public float blackScreenFade = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowCo());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b,
            Mathf.MoveTowards(blackScreen.color.a, 0f, blackScreenFade * Time.deltaTime));
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public IEnumerator ShowCo()
    {
        yield return new WaitForSeconds(timeBetweenShowing);

        firstText.SetActive(true);

        yield return new WaitForSeconds(timeBetweenShowing);

        secondText.SetActive(true);

        yield return new WaitForSeconds(timeBetweenShowing);

        returnButton.SetActive(true);
    }
}
