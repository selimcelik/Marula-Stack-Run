using UnityEngine;
using ManagerActorFramework;

public class GameManager : Manager<GameManager>
{
    // Info
    public bool IsGameStarted { get; private set; }
    public bool IsGameOver { get; private set; }

    // GameData : Entity
    public GameData gameData { get; private set; }

    public Transform startPos;
    public GameObject player;

    protected override void MB_Start()
    {
        // FPS
        Application.targetFrameRate = 60;

        // Init Game Data
        gameData = GameData.Get();
        if (gameData == null)
        {
            gameData = new GameData();
            bool isSuccess = gameData.Register();
            if (!isSuccess)
            {
                Debug.LogError("GameData Entity register error!");
            }
        }

        // Load game data
        gameData.Load();

        // Init game
        GameStatus_Init();
    }

    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            // Button Clicks
            Instance.Subscribe(ManagerEvents.BtnClick_Play, BtnClick_Play);
            Instance.Subscribe(ManagerEvents.BtnClick_Continue, BtnClick_Continue);
            Instance.Subscribe(ManagerEvents.BtnClick_SuccessBackToMenu, BtnClick_SuccessBackToMenu);
            Instance.Subscribe(ManagerEvents.BtnClick_Retry, BtnClick_Retry);
            Instance.Subscribe(ManagerEvents.BtnClick_FailBackToMenu, BtnClick_FailBackToMenu);

            // Others
            LevelManager.Instance.Subscribe(ManagerEvents.FinishLevel, FinishLevel);
        }
        else
        {
            // Button Clicks
            Instance.Unsubscribe(ManagerEvents.BtnClick_Play, BtnClick_Play);
            Instance.Unsubscribe(ManagerEvents.BtnClick_Continue, BtnClick_Continue);
            Instance.Unsubscribe(ManagerEvents.BtnClick_SuccessBackToMenu, BtnClick_SuccessBackToMenu);
            Instance.Unsubscribe(ManagerEvents.BtnClick_Retry, BtnClick_Retry);
            Instance.Unsubscribe(ManagerEvents.BtnClick_FailBackToMenu, BtnClick_FailBackToMenu);

            // Others
            LevelManager.Instance.Unsubscribe(ManagerEvents.FinishLevel, FinishLevel);
        }
    }

    #region Button Clicks

    private void BtnClick_Play(object[] args)
    {
        GameStatus_Start();
        PlayerMovement.instance.speed = 10;
        PlayerMovement.instance.StopAllCoroutines();
        player.transform.localPosition = startPos.position;
        player.transform.Rotate(0, 0, 0);
    }

    private void BtnClick_Continue(object[] args)
    {
        GameStatus_Restart(false);
        PlayerMovement.instance.speed = 10;
        PlayerMovement.instance.StopAllCoroutines();
        player.transform.localPosition = startPos.position;
        player.transform.Rotate(0, 0, 0);
    }

    private void BtnClick_SuccessBackToMenu(object[] args)
    {
        GameStatus_Restart(true);
    }

    private void BtnClick_Retry(object[] args)
    {
        GameStatus_Restart(false);
        PlayerMovement.instance.speed = 10;
        PlayerMovement.instance.StopAllCoroutines();
        player.transform.localPosition = startPos.position;
        player.transform.Rotate(0, 0, 0);
    }

    private void BtnClick_FailBackToMenu(object[] args)
    {
        GameStatus_Restart(true);
    }

    #endregion


    #region Game Status

    private void GameStatus_Init()
    {
        Publish(ManagerEvents.GameStatus_Init, gameData);
    }

    private void GameStatus_Start()
    {
        if (IsGameStarted)
        {
            return;
        }

        IsGameStarted = true;
        IsGameOver = false;

        Publish(ManagerEvents.GameStatus_Start);

        // Analytics
        AnalyticsManager.Instance.StartLevel(gameData.Level + 1);
    }

    private void GameStatus_GameOver(bool isSuccess)
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameStarted = true;
        IsGameOver = true;

        Publish(ManagerEvents.GameStatus_GameOver, isSuccess);

        if (isSuccess)
        {
            // Analytics
            AnalyticsManager.Instance.CompletedLevel(gameData.Level + 1);

            // New level
            gameData.UpdateLevel(gameData.Level + 1);
        }
        else
        {
            // Analytics
            AnalyticsManager.Instance.FailLevel(gameData.Level + 1);
        }
    }

    private void GameStatus_Restart(bool backToMenu)
    {
        if (!IsGameOver)
        {
            return;
        }

        IsGameStarted = false;
        IsGameOver = false;

        Publish(ManagerEvents.GameStatus_Restart);

        // Init game
        GameStatus_Init();

        // Continue
        if (!backToMenu)
        {
            GameStatus_Start();
        }
    }

    #endregion

    #region Other Events

    private void FinishLevel(object[] args)
    {
        bool isSuccess = (bool)args[0];

        GameStatus_GameOver(isSuccess);
    }

    #endregion

}
