using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerEvents;

public class SessionManager : MonoBehaviour
{

    private string user_id = null;
    private ServerEventManager event_manager;
    private const string url = "http://tfg.padaonegames.com/event";
    private const string game_name = "Unpossible";

    private const string DISTANCE = "DISTANCE";
    private static SessionManager _instance;
    public static SessionManager Instance
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
    #region SESSION_MENU
    public bool IsSessionInitialized()
    {
        return user_id != null;
    }

    public void SetUser(string user)
    {
        user_id = user;
    }
    #endregion

    #region EVENTS
    public void LogDeath(float distance, int score)
    {
        ServerEventParameter[] param = { new ServerEventParameter(DISTANCE, distance.ToString()),
                                         new ServerEventParameter(ServerEventParameter.SCORE, score.ToString()) };
        LogEvent(ServerEvent.PLAYER_DEATH, param);
    }

    public void LogTutorialStart()
    {
        LogEvent(ServerEvent.TUTORIAL_START);
    }

    public void LogTutorialEnd()
    {
        LogEvent(ServerEvent.TUTORIAL_END);
    }

    public void LogExperimentStart()
    {
        LogEvent(ServerEvent.EXPERIMENT_START);
    }

    public void LogExperimentEnd()
    {
        LogEvent(ServerEvent.EXPERIMENT_END);
    }
    #endregion

    #region PRIVATE_FUNCTIONS
    private void LogEvent(string event_name, ServerEventParameter[] param = null)
    {
        ServerEvent e = new ServerEvent(user_id, CurrentTimestamp(), game_name, event_name, param);
        Debug.Log("logging event: " + event_name);
        event_manager.LogEvent(e);
    }

    private int CurrentTimestamp()
    {
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }
    #endregion
}
