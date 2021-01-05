using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_loop : MonoBehaviour
{
    [SerializeField] private float scrollspeed;
    private float offset;
    private Material mat;
    [SerializeField] private Renderer background;
    [SerializeField] private string sortingLayer;
    [SerializeField] private int sortingOrder;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
        SetLayer();
    }

    private void Update()
    {
        offset += (Time.deltaTime * scrollspeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }

    public void SetLayer()
    {
        if (background == null)
        {
            background = this.GetComponent<Renderer>();
        }

        background.sortingLayerName = sortingLayer;
        background.sortingOrder = sortingOrder;
    }
}
