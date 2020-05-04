using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WebMenuManager : MonoBehaviour
{

    public GameObject TutorialMenu;
    public GameObject ExperimentMenu;
 
    #region INSTRUCTIONS
    public GameObject Instructions1;
    public GameObject Instructions2;
    #endregion

    public void Start()
    {
        TutorialMenu.SetActive(false);
        ExperimentMenu.SetActive(false);
        Instructions1.SetActive(false);
        Instructions2.SetActive(false);

        if (GameController.Instance.IsTutorialCompleted())
        {
            ExperimentMenu.SetActive(true);
        }
        else
        {
            SceneChanger.Instance.FadeIn();
            Instructions1.SetActive(true);
        }
    }
    
    public void StartLevel()
    {
        SceneChanger.Instance.LoadScene(SceneChanger.EXPERIMENT);
    }

    public void StartTutorial()
    {
        SceneChanger.Instance.LoadScene(SceneChanger.TUTORIAL);
    }

    #region INSTRUCTIONS
    public void Instructions1Next()
    {
        Instructions1.SetActive(false);
        Instructions2.SetActive(true);
    }

    public void Instructions2Next()
    {
        Instructions2.SetActive(false);
        
        TutorialMenu.SetActive(true);
    }
    #endregion
}
