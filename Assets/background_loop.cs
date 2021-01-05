using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background_loop : MonoBehaviour
{
    [SerializeField] private float scrollspeed;
    private float offset;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
       
    }

    private void Update()
    {
        offset += (Time.deltaTime * scrollspeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }

  
}
