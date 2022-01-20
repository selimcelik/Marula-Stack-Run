using UnityEngine;
using ManagerActorFramework;
using GameAnalyticsSDK;
using ElephantSDK;

public class AnalyticsManager : Manager<AnalyticsManager>
{
    protected override void MB_Start()
    {
        GameAnalytics.Initialize();
    }

    public void StartLevel(int level)
    {
        string s = level.ToString();
        if (level < 10)
        {
            s = $"0{level}";
        }
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level " + s);

        // Elephant SDK
        Elephant.LevelStarted(level);
    }

    public void CompletedLevel(int level)
    {
        string s = level.ToString();
        if (level < 10)
        {
            s = $"0{level}";
        }
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level " + s);

        // Elephant SDK
        Elephant.LevelCompleted(level);
    }

    public void FailLevel(int level)
    {
        string s = level.ToString();
        if (level < 10)
        {
            s = $"0{level}";
        }
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level " + s);

        // Elephant SDK
        Elephant.LevelFailed(level);
    }

    public void BtnClickEvent(string buttonName)
    {
        GameAnalytics.NewDesignEvent("Button:" + "Clicked:" + buttonName);
    }

    public void SoundStatus(bool soundStatus)
    {
        GameAnalytics.NewDesignEvent("Sound:" + "Status:" + soundStatus.ToString());
    }

    public void VibrationStatus(bool vibrationStatus)
    {
        GameAnalytics.NewDesignEvent("Vibrate:" + "Status:" + vibrationStatus.ToString());
    }
}
