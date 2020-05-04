using System;


[Serializable]
public class WebEventParameter
{
    public string name;
    public string value;

    public WebEventParameter(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

    #region parameter_constants
    public const string LEVEL_NUMBER = "LEVEL_NUMBER";
    public const string LOCATION_X = "LOCATION_X";
    public const string LOCATION_Y = "LOCATION_Y";
    public const string LOCATION_Z = "LOCATION_Z";
    public const string SCORE = "SCORE";
    #endregion
}

[Serializable]
public class WebEvent
{
    public int timestamp;
    public string gameName;
    public string name;
    public int orderInSequence;
    public WebEventParameter[] parameters;

    #region event_constants
    public const string EXPERIMENT_START = "EXPERIMENT_START";
    public const string EXPERIMENT_END = "EXPERIMENT_END";
    public const string TUTORIAL_START = "TUTORIAL_START";
    public const string TUTORIAL_END = "TUTORIAL_END";
    public const string LEVEL_START = "LEVEL_START";
    public const string LEVEL_END = "LEVEL_END";
    public const string PLAYER_DEATH = "PLAYER_DEATH";
    public const string GAME_PAUSE = "GAME_PAUSE";
    public const string GAME_RESUME = "GAME_RESUME";
    #endregion
}