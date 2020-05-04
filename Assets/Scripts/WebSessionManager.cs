using System.Collections.Generic;
using UnityEngine;
using ServerEvents;
using System.Runtime.InteropServices;
using System;

public class WebSessionManager : MonoBehaviour
{

    private string user_id = null;
    private ServerEventManager event_manager;
    private const string url = "https://intelligence-assessment-tfg.herokuapp.com";
    private const string game_name = "Unpossible";
    private int order_in_sequence = 0;

    private const string DISTANCE = "DISTANCE";

    [DllImport("__Internal")]
    private static extern void LogGameEvent(string eventJSON);

    [DllImport("__Internal")]
    private static extern void GameOver();

    private static WebSessionManager _instance;
    public static WebSessionManager Instance
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
            event_manager = new ServerEventManager(url);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region EVENTS
    
    public void LogDeath(float distance, int score)
    {
        WebEventParameter[] param = { new WebEventParameter(DISTANCE, distance.ToString()),
                                         new WebEventParameter(WebEventParameter.SCORE, score.ToString()) };
        LogEvent(WebEvent.PLAYER_DEATH, param);
    }

    public void LogTutorialStart()
    {
        LogEvent(WebEvent.TUTORIAL_START);
    }

    public void LogTutorialEnd()
    {
        LogEvent(WebEvent.TUTORIAL_END);
    }

    public void LogExperimentStart()
    {
        LogEvent(WebEvent.EXPERIMENT_START);
    }

    public void LogExperimentEnd()
    {
        LogEvent(WebEvent.EXPERIMENT_END);
    }

    public void EndGame()
    {
        
        GameOver();
    }

    #endregion

    private void LogEvent(string eventName, WebEventParameter[] parameters = null)
    {
        WebEvent webEvent = new WebEvent
        {
            name = eventName,
            parameters = parameters,
            timestamp = CurrentTimestamp(),
            gameName = game_name,
            orderInSequence = order_in_sequence
        };
        order_in_sequence++;
        LogGameEvent(JsonUtility.ToJson(webEvent));
    }
    private int CurrentTimestamp()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
}
