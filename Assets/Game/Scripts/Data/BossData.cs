using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Create Boss", fileName = "New Boss")]
public class BossData : ScriptableObject
{
    public bool wasDie;
    public Sprite bossLoseImage;
}
