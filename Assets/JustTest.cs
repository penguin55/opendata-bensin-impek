using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.DOShakePosition(5, 2, 90);
    }

    
}
