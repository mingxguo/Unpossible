﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject SettingsMenu;

    public void StartLevel()
    {
        Debug.Log("start");
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        Debug.Log("settings");
        gameObject.SetActive(false);
        SettingsMenu.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
