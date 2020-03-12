using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject SettingsMenu;

    public void StartLevel()
    {
        Debug.Log("start");
        SceneManager.LoadScene(2);
        GameController.Instance.OnLevelStart();
    }

    public void StartTutorial()
    {
        Debug.Log("start");
        SceneManager.LoadScene(1);
        GameController.Instance.OnLevelStart();
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
