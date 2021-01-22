using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TomWill;
using UnityEngine;
public class PlatformShipBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject objectFloat;


    private void Start()
    {
        Floating();
    }

    void Floating()
    {
        DOTween.Sequence()
            .Append(objectFloat.transform.DOLocalMoveY(-0.15f, 2.5f).SetEase(Ease.InOutBack))
            //.Join(objectFloat.transform.DOLocalRotate(Vector3.forward * -.5f, 5f).SetEase(Ease.InOutBack))
            .OnComplete(() =>
            {
                DOTween.Sequence()
                    .Append(objectFloat.transform.DOLocalMoveY(0.15f, 5f).SetEase(Ease.InOutBack))
                    //.Join(objectFloat.transform.DOLocalRotate(Vector3.forward * .5f, 5f).SetEase(Ease.InOutBack))
                    .Append(objectFloat.transform.DOLocalMoveY(-0.15f, 5f).SetEase(Ease.InOutBack))
                    //.Join(objectFloat.transform.DOLocalRotate(Vector3.forward * -.5f, 5f).SetEase(Ease.InOutBack))
                    .SetLoops(-1, LoopType.Yoyo);
            });
    }

}
