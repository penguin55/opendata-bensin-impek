using DG.Tweening;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] guns;
    [SerializeField] private Transform[] starts;
    [SerializeField] private Transform[] finals;
    [SerializeField] private float fallDuration, nextGunDuration;
    [SerializeField] private bool eachGun;

    public void StartGunEntrance()
    {
        if (eachGun) SequenceEach();
        else SequenceAll();
    }

    public void ResetToEntranceState()
    {
        for (int i = 0; i<guns.Length; i++)
        {
            guns[i].transform.position = starts[i].position;
            guns[i].transform.localScale = starts[i].localScale;
        }
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

        for (int i = 0; i < guns.Length; i++)
        {
            sequence.Join(guns[i].transform.DOMove(finals[i].position, fallDuration).SetEase(Ease.InExpo));
            sequence.Join(guns[i].transform.DOScale(finals[i].localScale, fallDuration).SetEase(Ease.InExpo));
        }
        sequence.OnComplete(() => CameraShake.instance.Shake(1, 2, 10));

        sequence.Play();
    }
}
