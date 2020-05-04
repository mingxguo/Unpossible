using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class SceneChanger : MonoBehaviour
{

    //public Animator animator;
    public Animation anim;

    public static readonly int MAIN = 0;
    public static readonly int TUTORIAL = 1;
    public static readonly int EXPERIMENT = 2;

    private static SceneChanger _instance;
    public static SceneChanger Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
{
    if (_instance == null)
    {
        _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        FadeIn();
    }

    public void FadeIn()
    {
        //animator.SetTrigger("FadeIn");
        anim.Play("FadeIn");
        //StartCoroutine(WaitForAnimation());
    }

    public void FadeOut()
    {
        //animator.SetTrigger("FadeOut");
        anim.Play("FadeOut");
        //StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        do
        {
            yield return null;
        } while (anim.isPlaying);
    }

    public void LoadScene(int scene)
    {
        FadeOut();
        SceneManager.LoadScene(scene);
    }
}
