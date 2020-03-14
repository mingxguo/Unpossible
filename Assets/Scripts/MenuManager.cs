using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject SettingsMenu;
    public GameObject MainMenu;
    public GameObject SessionMenu;


    private Slider player_speed_slider;
    private Slider rotate_speed_slider;

    private TextMeshProUGUI control_text;

    public void Start()
    {
        control_text = GameObject.Find("ControlText").GetComponent<TextMeshProUGUI>();
        Debug.Log(control_text == null);
        Slider[] sliders = SettingsMenu.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "PlayerSpeed")
            {
                player_speed_slider = s;
                Debug.Log("Found " + s.name);
            }
            else
            {
                rotate_speed_slider = s;
                Debug.Log("Found " + s.name);
            }
        }

        StartMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(false);
        SessionMenu.SetActive(false);
    }

    public void StartGame()
    {
        StartMenu.SetActive(false);
        SessionMenu.SetActive(true);
    }

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

    public void ShowSettingsMenu()
    {
        Debug.Log("settings");
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        SetSettingsControlText();
    }

    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    private void SetSettingsControlText()
    {
        if (GameController.Instance.IsTapControl())
        {
            control_text.text = "Tap";
        }
        else
        {
            control_text.text = "Tilt";
        }
    }

    public void ChangeControl()
    {
        if (GameController.Instance.IsTapControl())
        {
            GameController.Instance.SetTapControl(false);
        }
        else
        {
            GameController.Instance.SetTapControl(true);
        }
        SetSettingsControlText();
    }

    public void ConfirmSettings()
    {
        GameController.Instance.SetPlayerSpeed(player_speed_slider.value);
        GameController.Instance.SetRotateSpeed(rotate_speed_slider.value);
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void ConfirmUser()
    {
        SessionMenu.SetActive(false);
        MainMenu.SetActive(true);

        // TODO: session
    }
}
