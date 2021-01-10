using DG.Tweening;
using UnityEngine;

public class ChariotTweening : MonoBehaviour
{
    [SerializeField] private Transform start_position;
    [SerializeField] private Transform middle_position;
    [SerializeField] private Transform out_position;

    private float time;

    public void MoveTrain(string position, float time = 1)
    {
        this.time = time;

        switch (position.ToLower())
        {
            case "start":
                break;
            case "middle":
                break;
            case "out":
                break;
        }
    }

    public void MoveTrain(Vector2 position, float time = 1)
    {
        transform.DOMove(position, time).SetEase(Ease.Linear);
    }

    private void Move_Start()
    {
        transform.DOMove(start_position.position, time).SetEase(Ease.Linear);
    }

    private void Move_Middle()
    {
        transform.DOMove(middle_position.position, time).SetEase(Ease.Linear);
    }

    private void Move_Out()
    {
        transform.DOMove(out_position.position, time).SetEase(Ease.Linear);
    }
}
