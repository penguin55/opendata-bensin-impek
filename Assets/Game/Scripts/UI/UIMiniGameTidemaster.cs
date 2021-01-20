using UnityEngine;
using UnityEngine.UI;

public class UIMiniGameTidemaster : MonoBehaviour
{
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private Image rightButtonActivated;
    [SerializeField] private Image bottomButtonActivated;
    [SerializeField] private Image leftButtonActivated;
    [SerializeField] private Image topButtonActivated;
    [SerializeField] private Image timeFill;

    [System.Serializable]
    public struct SpriteDataShield
    {
        public Sprite active;
        public Sprite inactive;
    }

    [Header("Sprite Detail")]
    [SerializeField] private SpriteDataShield[] spriteDatas;

    public void ActivateMiniGame(bool flag, bool rightActivate = false, bool bottomActivate = false, bool leftActivate = false, bool topActivate = false)
    {
        miniGamePanel.SetActive(flag);

        ActivateRightButton(rightActivate);
        ActivateBottomButton(bottomActivate);
        ActivateLeftButton(leftActivate);
        ActivateTopButton(topActivate);
    }

    public void ActivateRightButton(bool flag)
    {
        if (flag) rightButtonActivated.sprite = spriteDatas[0].active;
        else rightButtonActivated.sprite = spriteDatas[0].inactive;
    }

    public void ActivateBottomButton(bool flag)
    {
        if (flag) bottomButtonActivated.sprite = spriteDatas[1].active;
        else bottomButtonActivated.sprite = spriteDatas[1].inactive;
    }

    public void ActivateLeftButton(bool flag)
    {
        if (flag) leftButtonActivated.sprite = spriteDatas[2].active;
        else leftButtonActivated.sprite = spriteDatas[2].inactive;
    }

    public void ActivateTopButton(bool flag)
    {
        if (flag) topButtonActivated.sprite = spriteDatas[3].active;
        else topButtonActivated.sprite = spriteDatas[3].inactive;
    }

    public void UpdateTimerInfo(float baseTime, float time)
    {
        //timerInfo.text = "Torpedo Launch in " + (int) time - 1 + " seconds";
        //timeFill.fillAmount = time / baseTime;
        float offset = time / baseTime; ;
        Vector2 scaleFill = new Vector2(offset, 1f);
        timeFill.rectTransform.localScale = scaleFill;
    }
}
