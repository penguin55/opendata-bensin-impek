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

    public static CameraShake instance;
    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void Shake(float duration, float strength, int vibrato)
    {
        this.duration = duration;
        this.strength = strength;
        this.vibrato = vibrato;
        main.DOShakeRotation(this.duration, this.strength, this.vibrato);
    }

}
