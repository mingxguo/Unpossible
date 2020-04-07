using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private Toggle playmode;
    public TextMeshProUGUI control_text;
    #endregion

    #region SESSION_MENU
    public TMP_InputField user_input;
    public GameObject InputPanel;
    public GameObject ErrorPanel;
    #endregion

    #region INSTRUCTIONS
    public GameObject Instructions1;
    public GameObject Instructions2;
    #endregion

    public void Start()
    {
        //control_text = SettingsMenu.transform.Find("ControlText").GetComponent<TextMeshProUGUI>();
        //user_input = SessionMenu.transform.Find("UserInput").GetComponent<TMP_InputField>();
        //InputPanel = SessionMenu.transform.Find("InputPanel").gameObject;
        //ErrorPanel = SessionMenu.transform.Find("ErrorPanel").gameObject;
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
        playmode = SettingsMenu.GetComponentInChildren<Toggle>();

        ErrorPanel.SetActive(false);
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(false);
        SessionMenu.SetActive(false);
        StartMenu.SetActive(false);
        Instructions1.SetActive(false);
        Instructions2.SetActive(false);

        if (SessionManager.Instance.IsSessionInitialized())
        {
            MainMenu.SetActive(true);
        }
        else
        {
            StartMenu.SetActive(true);
        }

        //SceneChanger.Instance.FadeIn();
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
        SceneChanger.Instance.LoadScene(SceneChanger.EXPERIMENT);
    }

    public void StartTutorial()
    {
        SceneChanger.Instance.LoadScene(SceneChanger.TUTORIAL);
    }

    public void ShowSettingsMenu()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);
        UpdateSettingsMenu();
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
    private void UpdateSettingsMenu()
    {
        // Set developer mode
        playmode.isOn = GameController.Instance.IsDeveloperMode();
        // Set control text
        if (GameController.Instance.IsTapControl())
        {
            control_text.text = "Tap";
        }
        else
        {
            control_text.text = "Tilt";
        }
        // Set slider values
        player_speed_slider.value = GameController.Instance.GetStartPlayerSpeed();
        rotate_speed_slider.value = GameController.Instance.GetStartRotateSpeed();
    }

    public void ChangeControl()
    {
        if (GameController.Instance.IsTapControl())
        {
            GameController.Instance.SetTapControl(false);
            control_text.text = "Tilt";
        }
        else
        {
            GameController.Instance.SetTapControl(true);
            control_text.text = "Tap";
        }
    }

    public void ChangeDeveloperMode()
    {
        Debug.Log("changing");
        GameController.Instance.SetDeveloperMode(playmode.isOn);
    }

    public void ConfirmSettings()
    {
        GameController.Instance.SetStartPlayerSpeed(player_speed_slider.value);
        GameController.Instance.SetStartRotateSpeed(rotate_speed_slider.value);
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
            InputPanel.SetActive(false);
            ErrorPanel.SetActive(true);
        }
        else
        {
            SceneChanger.Instance.FadeOut();
            SessionMenu.SetActive(false);
            Instructions1.SetActive(true);
            Debug.Log("user: " + user_input.text);
            SessionManager.Instance.SetUser(user_input.text);
            SceneChanger.Instance.FadeIn();
        }
    }

    public void ConfirmError()
    {
        ErrorPanel.SetActive(false);
        InputPanel.SetActive(true);
    }
    #endregion

    #region INSTRUCTIONS
    public void Instructions1Next()
    {
        Instructions1.SetActive(false);
        Instructions2.SetActive(true);
    }

    public void Instructions2Next()
    {
        Instructions2.SetActive(false);
        MainMenu.SetActive(true);
    }
    #endregion
}
