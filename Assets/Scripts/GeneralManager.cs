public static class GeneralManager 
{
    public static bool TimerActive = false;
    public static float countdownInitial;
    public static float countdown;
    public static int pointsP1 = 0;
    public static int pointsP2 = 0;
    public static bool IsCharselectSceneActive = false;
    public static bool CharSelectionShowedOnce = false;
    public static int yourPoints = 0;

    public static void Reset()
    {
        TimerActive = false;
        countdownInitial = 0;
        countdown = 0;
        pointsP1 = 0;
        pointsP2 = 0;
        IsCharselectSceneActive = false;
        CharSelectionShowedOnce = false;
    }
}
