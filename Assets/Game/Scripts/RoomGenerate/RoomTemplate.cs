using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    [SerializeField] private Room[] rooms;

    public Room GetRandomRoom()
    {
        int randomPick = Random.Range(0, rooms.Length);
        return rooms[randomPick];
    }
}
