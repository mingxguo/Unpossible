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

    #region SETTINGS_MENU
    private Slider player_speed_slider;
    private Slider rotate_speed_slider;
    public TextMeshProUGUI control_text;
    #endregion

    #region SESSION_MENU
    public TMP_InputField user_input;
    public GameObject input_panel;
    public GameObject error_panel;
    #endregion

    public void Start()
    {
        //control_text = SettingsMenu.transform.Find("ControlText").GetComponent<TextMeshProUGUI>();
        //user_input = SessionMenu.transform.Find("UserInput").GetComponent<TMP_InputField>();
        //input_panel = SessionMenu.transform.Find("InputPanel").gameObject;
        //error_panel = SessionMenu.transform.Find("ErrorPanel").gameObject;
        
        Slider[] sliders = SettingsMenu.GetComponentsInChildren<Slider>();
        foreach (Slider s in sliders)
        {
            if (s.name == "PlayerSpeed")
            {
                player_speed_slider = s;
            }
            else
            {
                rotate_speed_slider = s;
            }
        }

        error_panel.SetActive(false);
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(false);
        SessionMenu.SetActive(false);
        StartMenu.SetActive(false);
        if(SessionManager.Instance.IsSessionInitialized())
        {
            MainMenu.SetActive(true);
        }
        else
        {
            StartMenu.SetActive(true);
        }
    }

    #region START_MANU
    public void StartGame()
    {
        StartMenu.SetActive(false);
        SessionMenu.SetActive(true);
    }
    #endregion

    #region MAIN_MENU
    public void StartLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void StartTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowSettingsMenu()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        SetSettingsControlText();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeUser()
    {
        MainMenu.SetActive(false);
        SessionMenu.SetActive(true);
    }
    #endregion

    #region SETTINGS_MENU
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
    #endregion

    #region SESSION_MENU
    public void ConfirmUser()
    {
        if (user_input.text == "")
        {
            Debug.Log("error");
            input_panel.SetActive(false);
            error_panel.SetActive(true);
        }
        else
        {
            SessionMenu.SetActive(false);
            MainMenu.SetActive(true);
            Debug.Log("user: " + user_input.text);
            SessionManager.Instance.SetUser(user_input.text);
        }
    }

    public void ConfirmError()
    {
        error_panel.SetActive(false);
        input_panel.SetActive(true);
    }
    #endregion
}
