using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTest : MonoBehaviour
{
    public GameObject[] guns;
    public Transform[] starts;
    public Transform[] finals;
    public float fallDuration;
    public float nextGunDuration;
    public bool eachGun;

    // Start is called before the first frame update
    void Start()
    {
        if (eachGun) SequenceEach();
        else SequenceAll();
    }

    private void SequenceEach()
    {
        int index = 0;
        DOVirtual.DelayedCall(nextGunDuration, () =>
        {
            if (index < guns.Length)
            {
                DOTween.Sequence()
                .Append(guns[index].transform.DOMove(finals[index].position, fallDuration).SetEase(Ease.InExpo))
                .Join(guns[index].transform.DOScale(finals[index].localScale, fallDuration).SetEase(Ease.InExpo))
                .OnComplete(() =>
                {
                    CameraShake.instance.Shake(1, 2, 10);
                });
            }
            index++;
        }).SetLoops(guns.Length);
    }

    private void SequenceAll()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Pause();

        for (int i = 0; i<guns.Length; i++)
        {
            sequence.Join(guns[i].transform.DOMove(finals[i].position, fallDuration).SetEase(Ease.InExpo));
            sequence.Join(guns[i].transform.DOScale(finals[i].localScale, fallDuration).SetEase(Ease.InExpo));
        }
        sequence.OnComplete(() => CameraShake.instance.Shake(1, 2, 10));

        sequence.Play();
    }
}
