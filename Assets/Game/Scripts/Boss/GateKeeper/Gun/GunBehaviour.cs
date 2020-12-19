using DG.Tweening;
using TomWill;
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
            guns[i].GetComponent<SpriteRenderer>().sortingLayerName = "Fore";
        }
    }

    private void SequenceEach(int index)
    {
        guns[index].transform.DOMove(finals[index].position, fallDuration).SetEase(Ease.InExpo).OnComplete(()=>
        {
            guns[index].GetComponent<SpriteRenderer>().sortingLayerName = "Game";
            TWAudioController.PlaySFX("SFX_BOSS", "rocket_impact");
            CameraShake.instance.Shake(1, 2, 10);
        });
        guns[index].transform.DOScale(finals[index].localScale, fallDuration).SetEase(Ease.InExpo);
    }

    private void SequenceEach()
    {
        // Terjadi race condition, maka dari itu diubah sequence manual

        int index = 0;
        DOVirtual.DelayedCall(nextGunDuration, () =>
        {
            SequenceEach(index);
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
            sequence.Join(guns[i].transform.DOScale(finals[i].localScale, fallDuration).SetEase(Ease.InExpo)
                .OnComplete(()=> guns[i].GetComponent<SpriteRenderer>().sortingLayerName = "Game"));
        }
        sequence.OnComplete(() => CameraShake.instance.Shake(1, 2, 10));

        sequence.Play();
    }
}
