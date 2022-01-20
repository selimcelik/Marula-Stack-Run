public enum ManagerEvents
{
    // Game Status
    GameStatus_Init,
    GameStatus_Start,
    GameStatus_GameOver,
    GameStatus_Restart,

    // Button
    BtnClick_Play,
    BtnClick_Continue,
    BtnClick_SuccessBackToMenu,
    BtnClick_Retry,
    BtnClick_FailBackToMenu,

    // Others,
    InitLevel,
    FinishLevel,
    UpdateScore,

    TutorialCompleted,
}
