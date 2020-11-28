using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTest : MonoBehaviour
{
    public Transform to, from;
    public Transform objecta;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.DOShakePosition(5, 2, 90);
    }

    private void Update()
    {
        Sizing();
    }


    void Sizing()
    {
        objecta.localScale = new Vector3((to.position-from.position).magnitude - 1.441f, objecta.localScale.y, objecta.localScale.z);
    }
    
}
