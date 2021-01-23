using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertWarning : MonoBehaviour
{
    [SerializeField] private Image warningPanel;

    public void ActiveAlert(bool flag)
    {
        if (flag)
        {
            DOTween.Sequence()
                .Append(warningPanel.DOFade(.5f, 1))
                .Append(warningPanel.DOFade(0, 1))
                .SetLoops(-1, LoopType.Restart)
                .SetId("warningAlert");
        }
        else
        {
            DOTween.Kill("warningAlert");
            warningPanel.DOFade(0, 1);
        }
    }
}
