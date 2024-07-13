using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameController : MonoBehaviour
{
    private GameObject player, gui;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gui = GameObject.FindGameObjectWithTag("GUI");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseButton();
        }
        else if(Input.GetKeyDown(KeyCode.Return))
        {
            if (gui.transform.GetChild(0).GetChild(5).gameObject.activeSelf)
            {
                ResumeButton();
            }
            else
            {
                RestartButton();
            }
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);
    }



    public void PauseButton()
    {
        gui.transform.GetChild(2).transform.gameObject.SetActive(false);
        gui.transform.GetChild(0).transform.gameObject.SetActive(true);
        gui.transform.GetChild(0).transform.gameObject.GetComponent<Animator>().enabled = false;
        gui.transform.GetChild(0).transform.position = Vector3.zero;
        Time.timeScale = 0;
    }

    public void ResumeButton()
    {
        gui.transform.GetChild(0).transform.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}
