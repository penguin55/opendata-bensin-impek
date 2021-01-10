using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderHandle : MonoBehaviour
{
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject objectView;

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            OrderHandle();
        }
    }

    private void OrderHandle()
    {
        if (!objectView && render == null) return; 

        if (transform.position.y < objectView.transform.position.y)
        {
            render.sortingOrder = 3;
        } else
        {
            render.sortingOrder = 0;
        }
    }
}
