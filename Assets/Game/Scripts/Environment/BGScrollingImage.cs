using UnityEngine;

public class BGScrollingImage : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 direction;
    private float panjangBG;

    private void Start()
    {
        panjangBG = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
        direction = Vector2.left;
        Destroy(GetComponent<BoxCollider2D>());
    }

    private void Update()
    {
        if (GameVariables.SLOW_MO)
        {
            transform.Translate(direction * speed * BGScrollingManager.instance.GlobalSpeed * GameTime.LocalTimeScale *Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * speed * BGScrollingManager.instance.GlobalSpeed * Time.deltaTime);
        }
        
        DeteksiBatas();
    }

    public void DeteksiBatas()
    {
        if(speed>0 && transform.localPosition.x <= -(panjangBG * 1.5f))
        {
            transform.localPosition = new Vector2(transform.localPosition.x + 3 * panjangBG, 0);
        }
        else if(speed<0 && transform.localPosition.x >= (panjangBG * 1.5f))
        {
            transform.localPosition = new Vector2(transform.localPosition.x - 3 * panjangBG, 0);
        }
    }
}
