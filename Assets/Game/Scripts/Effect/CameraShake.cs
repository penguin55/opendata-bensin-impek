using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraShake : MonoBehaviour
{
    public Camera main;

    [SerializeField] private float duration;
    [SerializeField] private float strength;
    [SerializeField] private int vibrato;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Shake()
    {
        main.DOShakeRotation(duration, strength, vibrato);
    }
}
