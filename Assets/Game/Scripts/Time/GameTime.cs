using DG.Tweening;
using UnityEngine;

public static class GameTime
{
    private static float localTimeScale = 1;
    private static float playerTimeScale = 1;

    public static float LocalTime { get { return localTimeScale * Time.deltaTime; } }
    public static float PlayerTime { get { return playerTimeScale * Time.deltaTime; } }

    public static float GlobalTimeScale
    {
        set
        {
            playerTimeScale = value; 
            localTimeScale = value;
            DOTween.timeScale = localTimeScale;
        }
    }
    public static float LocalTimeScale
    {
        get { return localTimeScale; }
        set
        {
            localTimeScale = value;
            DOTween.timeScale = localTimeScale;
        }
    }
    public static float PlayerTimeScale { get { return playerTimeScale; } set { playerTimeScale = value; } }
}
