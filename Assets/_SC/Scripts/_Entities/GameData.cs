using UnityEngine;
using ManagerActorFramework;

public class GameData : Entity<GameData>
{
    #region Variables

    public int Level = 0;
    public int Score = 0;

    #endregion


    protected override bool Init()
    {
        return true;
    }

    public void UpdateLevel(int level)
    {
        Level = level;
        Save();
    }
    public void AddScore(int score)
    {
        Score += score;
        Save();
    }
}
