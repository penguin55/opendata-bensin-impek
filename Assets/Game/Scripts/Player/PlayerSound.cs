using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TomWill;
public class PlayerSound : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FootStep()
    {
        TWAudioController.PlaySFX("PLAYER_SFX", "player_move");
    }
}
