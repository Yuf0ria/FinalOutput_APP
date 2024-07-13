using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject UI_BG, death, player, kid, obstacles, tuto;
    public float tutorialseconds;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        if (tuto != null)
        {
            tutorialdialogue();
        }

        if (player == null)
        {
            DeathScreen();
        }
    }

    private void tutorialdialogue()
    {

        StartCoroutine(tutorial());
    }

    public void PauseGameButton()
    {
        UI_BG.SetActive(true);
        tuto.SetActive(false);
        Time.timeScale = 0;
    }
    public void ResumeGameButton()
    {
        UI_BG.SetActive(false);
        Time.timeScale = 1;
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }


    private void DeathScreen()
    {
        death.SetActive(true);
        kid.transform.position += new Vector3(2, 0, 0) * Time.deltaTime;
        obstacles.SetActive(false);
        tuto.SetActive(false);

    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator tutorial()
    {
        yield return new WaitForSeconds(tutorialseconds);
        tuto.SetActive(false);
    }

}
