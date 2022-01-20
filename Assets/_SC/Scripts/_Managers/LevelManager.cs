using UnityEngine;
using ManagerActorFramework.Utils;
//using UnityEngine.iOS;

public class LevelManager : MB_LevelManager<LevelManager>
{
    // Private variables
    private LevelActor levelActor;

    private bool tutorialCompleted;
    private bool levelFailed;

    protected override void MB_Start()
    {
        // Custom
    }

    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            // Game Status
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Init, GameStatus_Init);
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Start, GameStatus_Start);
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_GameOver, GameStatus_GameOver);
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Restart, GameStatus_Restart);
        }
        else
        {
            // Game Status
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_Init, GameStatus_Init);
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_Start, GameStatus_Start);
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_GameOver, GameStatus_GameOver);
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_Restart, GameStatus_Restart);
        }
    }

    #region Game Status

    private void GameStatus_Init(object[] args)
    {
        GameData gameData = (GameData)args[0];

        if (!levelFailed)
        {
            // Get level data
            LoadLevelData(gameData.Level);

            // Rate Us
            if (gameData.Level == 8)
            {
                /*#if UNITY_IOS
                Device.RequestStoreReview();
                #endif*/
            }
        }
        else
        {
            // Restart level
            RestartLevel();
        }

        // Spawn level actor
        levelActor = CurrentLevelData.InitiliazeLevel();
        levelActor.InitLevel();

        // Publish
        Publish(ManagerEvents.InitLevel, CurrentLevelData);
    }

    private void GameStatus_Start(object[] args)
    {
        tutorialCompleted = false;
    }

    private void GameStatus_GameOver(object[] args)
    {
        bool isSuccess = (bool)args[0];
        levelFailed = !isSuccess;
    }

    private void GameStatus_Restart(object[] args)
    {
        if (levelActor)
        {
            Destroy(levelActor.gameObject);
            levelActor = null;
        }
    }

    #endregion


    protected override void MB_Update()
    {
        if (!GameManager.Instance.IsGameStarted || GameManager.Instance.IsGameOver)
        {
            return;
        }

        // Mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Tutorial
            if (!tutorialCompleted)
            {
                tutorialCompleted = true;
                Publish(ManagerEvents.TutorialCompleted);
            }
        }

        // Just test
        if (Input.GetMouseButtonDown(1))
        {
            Publish(ManagerEvents.FinishLevel, true);
        }
    }
}
