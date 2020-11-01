using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager instance;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private RuntimeAnimatorController runtime;
    [SerializeField] private PlayableDirector director;


    public PlayableDirector Director { get => director; set => director = value; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
