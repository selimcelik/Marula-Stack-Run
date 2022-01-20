using UnityEngine;
using UnityEngine.UI;
using ManagerActorFramework.Utils;
using System.Collections;

public class UiActor : MB_UiActor<GameManager>
{
    [Header("# Panels #")]
    public GameObject menu_Home;
    public GameObject menu_InGame;
    public GameObject menu_Success;
    public GameObject menu_Fail;

    [Space(10)]
    public GameObject pnl_Header;

    [Header("Menu / Home")]
    public Button btn_Play;
    public Text HomeScoreText;

    [Header("Menu / InGame")]
    public GameObject obj_Tutorial;
    public Text InGameScoreText;

    [Header("Menu / Success")]
    public Button btn_Continue;
    public Button btn_Success_BackToMenu;
    public Text SuccessScoreText;
    public Text GainedScoreText;

    [Header("Menu / Fail")]
    public Button btn_Retry;
    public Button btn_Fail_BackToMenu;

    [Header("Panel / Header")]
    public Text txt_Level;

    [Header("# Level Completed Particle #")]
    public GameObject obj_LevelCompleted;

    [Header("Background Settings")]
    public bool changeBackgroundColor = true;
    public bool isSkybox = false;
    public Material mat_Background;

    public Button powerUpBtn;

    // Private variables
    private bool tutorialCompleted = false;

    protected override void MB_Awake()
    {
        menu_Home.SetActive(false);
        menu_InGame.SetActive(false);
        menu_Success.SetActive(false);
        menu_Fail.SetActive(false);

        pnl_Header.SetActive(false);

        obj_LevelCompleted.SetActive(false);
        obj_Tutorial.SetActive(false);
    }

    protected override void MB_Listen(bool status)
    {
        if (status)
        {
            // Button Clicks
            btn_Play.onClick.AddListener(BtnClick_Play);
            btn_Continue.onClick.AddListener(BtnClick_Continue);
            btn_Success_BackToMenu.onClick.AddListener(BtnClick_SuccessBackToMenu);
            btn_Retry.onClick.AddListener(BtnClick_Retry);
            btn_Fail_BackToMenu.onClick.AddListener(BtnClick_FailBackToMenu);

            // Game Status
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Init, GameStatus_Init);
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Start, GameStatus_Start);
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_GameOver, GameStatus_GameOver);
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Restart, GameStatus_Restart);

            // Others
            LevelManager.Instance.Subscribe(ManagerEvents.InitLevel, InitLevel);
            LevelManager.Instance.Subscribe(ManagerEvents.TutorialCompleted, TutorialCompleted);
        }
        else
        {
            // Button Clicks
            btn_Play.onClick.RemoveListener(BtnClick_Play);
            btn_Continue.onClick.RemoveListener(BtnClick_Continue);
            btn_Success_BackToMenu.onClick.RemoveListener(BtnClick_SuccessBackToMenu);
            btn_Retry.onClick.RemoveListener(BtnClick_Retry);
            btn_Fail_BackToMenu.onClick.RemoveListener(BtnClick_FailBackToMenu);

            // Game Status
            GameManager.Instance.Subscribe(ManagerEvents.GameStatus_Init, GameStatus_Init);
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_Start, GameStatus_Start);
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_GameOver, GameStatus_GameOver);
            GameManager.Instance.Unsubscribe(ManagerEvents.GameStatus_Restart, GameStatus_Restart);

            // Others
            LevelManager.Instance.Unsubscribe(ManagerEvents.InitLevel, InitLevel);
            LevelManager.Instance.Unsubscribe(ManagerEvents.TutorialCompleted, TutorialCompleted);
        }
    }

    private void Update()
    {
        HomeScoreText.text = GameManager.Instance.gameData.Score.ToString();

        InGameScoreText.text = GameManager.Instance.gameData.Score.ToString();

        SuccessScoreText.text = GameManager.Instance.gameData.Score.ToString();

        GainedScoreText.text = PlayerMovement.instance.gainedScore.ToString();
    }


    #region Game Status

    private void GameStatus_Init(object[] args)
    {
        GameData gameData = (GameData)args[0];

        // Show main screen
        ShowMenu_Home();

        // Update level bar
        txt_Level.text = $"LEVEL {gameData.Level + 1}";

        // Show level bar
        if (!pnl_Header.activeSelf)
        {
            pnl_Header.SetActive(true);
        }
    }

    private void GameStatus_Start(object[] args)
    {
        ShowMenu_InGame();
    }

    private void GameStatus_GameOver(object[] args)
    {
        bool isSuccess = (bool)args[0];
        if (isSuccess)
        {
            ShowMenu_Success();

            // Show particle
            obj_LevelCompleted.SetActive(true);
        }
        else
        {
            ShowMenu_Fail();
        }
    }

    private void GameStatus_Restart(object[] args)
    {
        obj_LevelCompleted.SetActive(false);
    }

    #endregion

    #region Other Events

    private void InitLevel(object[] args)
    {
        Level levelData = (Level)args[0];

        if (!changeBackgroundColor)
        {
            return;
        }

        if (isSkybox)
        {
            // Skybox color
            mat_Background.SetColor("_Color1", levelData.color_Background1);
            mat_Background.SetColor("_Color2", levelData.color_Background1);
        }
        else
        {
            mat_Background.color = levelData.color_Background1;
        }
    }

    private void TutorialCompleted(object[] args)
    {
        obj_Tutorial.SetActive(false);
        tutorialCompleted = true;
    }

    #endregion

    #region Open Panels

    private void ShowMenu_Home()
    {
        OpenPanel(menu_Home, MenuType.Menu);
    }

    private void ShowMenu_InGame()
    {
        OpenPanel(menu_InGame, MenuType.Menu);

        tutorialCompleted = false;
        //StartCoroutine(ShowTutorial());
    }

    private IEnumerator ShowTutorial()
    {
        yield return new WaitForSeconds(0.5f);

        if (tutorialCompleted)
        {
            yield break;
        }

        // Show tutorial
        obj_Tutorial.SetActive(true);
    }

    private void ShowMenu_Success()
    {
        OpenPanel(menu_Success, MenuType.Menu);
    }

    private void ShowMenu_Fail()
    {
        OpenPanel(menu_Fail, MenuType.Menu);
    }

    #endregion


    #region Button Clicks

    private void BtnClick_Play()
    {
        Push(ManagerEvents.BtnClick_Play);

        PlayerMovement.instance.startLevel = true;
        PlayerMovement.instance.levelEnd = false;
        PlayerMovement.instance.levelFailed = false;


        // Analytics
        AnalyticsManager.Instance.BtnClickEvent("BtnClick_Play");

        // Vibrate
        VibrationManager.Instance.TriggerLightImpact();
    }

    private void BtnClick_Continue()
    {
        Push(ManagerEvents.BtnClick_Continue);

        PlayerMovement.instance.startLevel = true;
        PlayerMovement.instance.levelEnd = false;
        PlayerMovement.instance.levelFailed = false;

        // Analytics
        AnalyticsManager.Instance.BtnClickEvent("BtnClick_Continue");

        // Vibrate
        VibrationManager.Instance.TriggerLightImpact();
    }

    private void BtnClick_SuccessBackToMenu()
    {
        /*Push(ManagerEvents.BtnClick_SuccessBackToMenu);

        // Analytics
        AnalyticsManager.Instance.BtnClickEvent("BtnClick_SuccessBackToMenu");

        // Vibrate
        VibrationManager.Instance.TriggerLightImpact();*/
    }

    private void BtnClick_Retry()
    {
        Push(ManagerEvents.BtnClick_Retry);

        PlayerMovement.instance.startLevel = true;
        PlayerMovement.instance.levelEnd = false;
        PlayerMovement.instance.levelFailed = false;

        // Analytics
        AnalyticsManager.Instance.BtnClickEvent("BtnClick_Retry");

        // Vibrate
        VibrationManager.Instance.TriggerLightImpact();
    }

    private void BtnClick_FailBackToMenu()
    {
        /*Push(ManagerEvents.BtnClick_FailBackToMenu);

        // Analytics
        AnalyticsManager.Instance.BtnClickEvent("BtnClick_FailBackToMenu");

        // Vibrate
        VibrationManager.Instance.TriggerLightImpact();*/
    }

    public void PowerUp()
    {
        StartCoroutine(PowerUpButtonInteractable());
        PlayerMovement.instance.powerUpBool = true;
        powerUpBtn.interactable = false;
    }

    IEnumerator PowerUpButtonInteractable()
    {
        yield return new WaitForSeconds(30);
        powerUpBtn.interactable = true;
    }

    #endregion


}
