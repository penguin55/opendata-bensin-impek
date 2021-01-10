using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrollingManager : MonoBehaviour
{
    public static BGScrollingManager instance;

    public float GlobalSpeed { get { return move ? 1.0f : 0.0f; } }

    [SerializeField] private bool move;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void MoveBackground(bool flag)
    {
        move = flag;
    }

    private void OnDisable()
    {
        instance = null;
    }
}
