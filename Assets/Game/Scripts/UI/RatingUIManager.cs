using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RatingUIManager : MonoBehaviour
{
    [SerializeField] private bool debuggingMode;
    [SerializeField, Range(0f,1f)] private float timeCountPercentage;
    [SerializeField] private float timePerStep;

    [SerializeField] private RectTransform ratingPanel;
    [SerializeField] private GameObject timerPanel;
    [SerializeField] private GameObject deathInfoPanel;
    [SerializeField] private GameObject ratingImagePanel;
    [SerializeField] private GameObject itemUsedPanel;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private Text timer;
    [SerializeField] private Text deathCount;
    [SerializeField] private Image ratingImage;
    [SerializeField] private Sprite[] rateStamps;
    
    [Header("Item UI")]
    [SerializeField] private GameObject placeholderItem;
    [SerializeField] private Transform contentViewList;
    [SerializeField] private ScrollRect scrollRect;
    private RectTransform elementAnchor;
    private float baseSize = 5;

    private Tween activeTween;
    private string rateType;

    private void Start()
    {
        TWLoading.OnSuccessLoad(()=>
        {
            TWTransition.ScreenTransition(TWTransition.TransitionType.DOWN_OUT, 1f, ()=> OpenPanelRating());
        });
    }

    private void Update()
    {
        if (activeTween != null && Input.GetKeyDown(KeyCode.Space))
        {
            activeTween.Complete();
        }
    }

    public void RenderRating()
    {
        /*DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(RenderTimerInfo)
            .AppendInterval((GameTrackRate.Time * timeCountPercentage) + 0.5f)
            .AppendCallback(RenderDeathInfo)
            .AppendInterval((GameTrackRate.DeathCount * timeCountPercentage) + 0.5f)
            .AppendCallback(RenderItemScroll)
            .AppendInterval(.5f)
            .AppendCallback(RenderRatingStamp)
            .AppendInterval(.5f)
            .AppendCallback(() => nextButton.SetActive(true));*/

        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(RenderTimerInfo);
    }

    public void GoToMainMenu()
    {
        TWAudioController.PlaySFX("UI", "click");
        TWTransition.ScreenTransition(TWTransition.TransitionType.DEFAULT_IN, .5f, () => TWLoading.LoadScene("MainMenu"));
        TWAudioController.PlaySFX("UI", "transition");
    }

    private void OpenPanelRating()
    {
        ratingPanel.gameObject.SetActive(true);

        CameraShake.instance.Shake(1, 3, 5);
        TWAudioController.PlaySFX("SFX", "rating_big_panel_appear");

        ratingPanel.DOShakeAnchorPos(1, 7, 9);

        if (!debuggingMode) RenderRating();
        else RatingDebugger.Instance.InitToGame(); // Debug purpose
    }

    private void RenderTimerInfo()
    {
        timerPanel.SetActive(true);
        TWAudioController.PlaySFX("SFX", "rating_appear");

        float targetCount = GameTrackRate.Time;
        int currentCount = -1;
        int hours = 0, minutes = 0, seconds = 0;

        activeTween = DOVirtual.Float(0, targetCount, targetCount * timeCountPercentage, (time)=>
        {
            if ((int) time != currentCount)
            {
                currentCount = (int)time;

                seconds = currentCount % 60;
                minutes = (currentCount / 60) % 60;
                hours = (currentCount / 60) / 60;

                timer.text = ": " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

                TWAudioController.PlaySFX("SFX", "rating_count_step");
            }
        }).OnComplete(()=>
        {
            activeTween = null;
            currentCount = (int) targetCount;

            seconds = currentCount % 60;
            minutes = (currentCount / 60) % 60;
            hours = (currentCount / 60) / 60;

            timer.text = ": " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

            DOVirtual.DelayedCall(timePerStep, RenderDeathInfo);
        })
        .SetEase(Ease.Linear);
    }

    private void RenderDeathInfo()
    {
        deathInfoPanel.SetActive(true);
        TWAudioController.PlaySFX("SFX", "rating_appear");

        int targetCount = GameTrackRate.DeathCount;
        int currentCount = -1;

        activeTween = DOVirtual.Float(0, targetCount, targetCount * timeCountPercentage, (time) =>
        {
            if ((int)time != currentCount)
            {
                currentCount = (int)time;

                deathCount.text = ": " + currentCount;
                TWAudioController.PlaySFX("SFX", "rating_count_step");
            }
        }).OnComplete(() =>
        {
            activeTween = null;
            currentCount = targetCount;

            deathCount.text = ": " + currentCount;

            DOVirtual.DelayedCall(timePerStep, RenderItemScroll);
        })
        .SetEase(Ease.Linear);
    }

    private void RenderRatingStamp()
    {
        ratingImagePanel.SetActive(true);

        int indexStamp = GetStampValue();

        switch (indexStamp)
        {
            case 0:
                rateType = "rating_stamp_s";
                break;
            case 1:
                rateType = "rating_stamp_a";
                break;
            case 2:
                rateType = "rating_stamp_b";
                break;
            default:
                rateType = "rating_stamp_c";
                break;
        }

        ratingImage.sprite = rateStamps[indexStamp];
        CameraShake.instance.Shake(1,3,5);
        TWAudioController.PlaySFX("SFX", rateType);

        ratingPanel.DOShakeAnchorPos(1, 7, 9);

        nextButton.SetActive(true);
    }

    private void RenderItemScroll()
    {
        itemUsedPanel.SetActive(true);
        TWAudioController.PlaySFX("SFX", "rating_appear");
        int itemsSize = GameTrackRate.ItemUsed != null ? GameTrackRate.ItemUsed.Count : 0;

        if (GameTrackRate.ItemUsed != null)
        {
            foreach (ItemData item in GameTrackRate.ItemUsed)
            {
                SetPlaceholder(item);
            }
        }

        SetStartPosition(itemsSize);

        DOVirtual.DelayedCall(timePerStep, RenderRatingStamp);
    }

    private void SetStartPosition(int itemSize)
    {
        if (itemSize > baseSize)
        {
            float widthModify = (elementAnchor.sizeDelta.x + 15f) * (itemSize - baseSize);
            scrollRect.content.sizeDelta = new Vector2(widthModify, scrollRect.content.sizeDelta.y);
            scrollRect.horizontalNormalizedPosition = 0.5f;
        }
    }

    private void SetPlaceholder(ItemData data)
    {
        GameObject instanceObject = Instantiate(placeholderItem, contentViewList);
        Image instanceImage = instanceObject.GetComponent<Image>();

        instanceImage.sprite = data.smallImage;
        if (!elementAnchor) elementAnchor = instanceObject.GetComponent<RectTransform>();
    }

    private int GetStampValue()
    {
        int stampTime = GetTimeStamp();
        int stampDeath = GetDeathStamp();

        int finalStamp = Mathf.FloorToInt((stampTime + stampDeath) / 2f);

        return finalStamp;
    }

    private int GetTimeStamp()
    {
        int stamp = -1;

        float timer = GameTrackRate.Time;

        if (timer <= 30f) stamp = 1;
        else if (timer > 30f && timer <= 45f) stamp = 2;
        else if (timer > 45f && timer <= 50f) stamp = 3;
        else stamp = 4;

        return stamp;
    }

    private int GetDeathStamp()
    {
        int stamp = -1;

        int deathCount = GameTrackRate.DeathCount;

        if (deathCount == 0) stamp = 0;
        else if (deathCount >= 1 && deathCount <= 2) stamp = 1;
        else if (deathCount >= 3 && deathCount <= 4) stamp = 2;
        else stamp = 3;

        return stamp;
    }
}
