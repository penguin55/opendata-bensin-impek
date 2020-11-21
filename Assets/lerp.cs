using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerp : MonoBehaviour
{
    public Vector2 beginning, end;
    public bool scale;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(scale) this.transform.localScale = Vector2.Lerp(transform.localScale, end, 2 * Time.deltaTime);
        else this.transform.localScale = Vector2.Lerp(transform.localScale, beginning, 2 * Time.deltaTime);
    }
}
