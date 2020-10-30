using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data instance;

    [SerializeField] private CharaBehaviour character;

    public CharaBehaviour Character { get => character; set => character = value; }

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }
}
