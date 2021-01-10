using DG.Tweening;
using UnityEngine;

public class ChariotTweening : MonoBehaviour
{
    [SerializeField] private Transform start_position;
    [SerializeField] private Transform middle_position;
    [SerializeField] private Transform end_position;
    [SerializeField] private Transform outStart_position;
    [SerializeField] private Transform outEnd_position;

    private float time;

    public void MoveTrain(string position, float time = 1)
    {
        this.time = time;

        switch (position.ToLower())
        {
            case "start":
                Move_Start();
                break;
            case "middle":
                Move_Middle();
                break;
            case "end":
                Move_End();
                break;
            case "out_start":
                Move_OutStart();
                break;
            case "out_end":
                Move_OutEnd();
                break;
        }
    }

    public void MoveTrain(Vector2 position, float time = 1)
    {
        transform.DOMove(position, time).SetEase(Ease.Linear);
    }

    public void MoveTrain(float xPosition, float time = 1)
    {
        transform.DOMoveX(transform.position.x + xPosition, time).SetEase(Ease.Linear);
    }

    private void Move_Start()
    {
        transform.DOMove(start_position.position, time).SetEase(Ease.Linear);
    }

    private void Move_Middle()
    {
        transform.DOMove(middle_position.position, time).SetEase(Ease.Linear);
    }

    private void Move_End()
    {
        transform.DOMove(end_position.position, time).SetEase(Ease.OutCubic);
    }

    private void Move_OutStart()
    {
        transform.DOMove(outStart_position.position, time).SetEase(Ease.Linear);
    }

    private void Move_OutEnd()
    {
        transform.DOMove(outEnd_position.position, time).SetEase(Ease.Linear);
    }
}
