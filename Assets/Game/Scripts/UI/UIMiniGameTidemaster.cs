using UnityEngine;
using UnityEngine.UI;

public class UIMiniGameTidemaster : MonoBehaviour
{
    [SerializeField] private GameObject miniGamePanel;
    [SerializeField] private Image rightButtonActivated;
    [SerializeField] private Image bottomButtonActivated;
    [SerializeField] private Image leftButtonActivated;
    [SerializeField] private Image topButtonActivated;
    [SerializeField] private Text timerInfo;
    [SerializeField] private Image timeFill;

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
        if (flag) rightButtonActivated.color = Color.white;
        else rightButtonActivated.color = Color.red;
    }

    public void ActivateBottomButton(bool flag)
    {
        if (flag) bottomButtonActivated.color = Color.white;
        else bottomButtonActivated.color = Color.red;
    }

    public void ActivateLeftButton(bool flag)
    {
        if (flag) leftButtonActivated.color = Color.white;
        else leftButtonActivated.color = Color.red;
    }

    public void ActivateTopButton(bool flag)
    {
        if (flag) topButtonActivated.color = Color.white;
        else topButtonActivated.color = Color.red;
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
