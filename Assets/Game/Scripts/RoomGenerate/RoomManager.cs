using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    //Note, yang ini belum kuoptimize dan masih berantakan. Nanti kuberesin
    public static RoomManager Instance;

    [SerializeField] private Vector2Int roomSize;
    [SerializeField] private RoomTemplate roomTemplate;

    [SerializeField] private Room testa;
    [SerializeField] private Transform parent;
    private MapLayout layout;

    private void Start()
    {
        Instance = this;

        layout = new MapLayout();
        layout.Init(roomSize.x, roomSize.y);
        layout.GenerateLayout();

        SpawningRoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            parent.DestroyChildrens();
            parent.localScale = Vector3.one;

            layout = new MapLayout();
            layout.Init(roomSize.x, roomSize.y);
            layout.GenerateLayout();

            SpawningRoom();
        }
    }

    private void SpawningRoom()
    {
        int[,] grid = layout.GetGridLayout();

        string text = "";

        for (int i = 0; i < roomSize.y; i++)
        {
            for (int j = 0; j < roomSize.x; j++)
            {
                text += grid[j, i] + " ";
                if (grid[j,i] != 0)
                {
                    Room spawn = Instantiate(testa, new Vector2(j, i), Quaternion.identity, parent);
                    spawn.Init(grid, j, i, grid[j,i]);

                    if ((j, i) == layout.GetStartNode()) spawn.SetStateRoom("START");
                    else if ((j, i) == layout.GetEndNode()) spawn.SetStateRoom("END");
                }
            }
            text += "\n";
        }

        parent.localScale = new Vector3(1, -1, 1);
        Debug.Log(text);
    }

    public (int x, int y) RoomSize()
    {
        return (roomSize.x , roomSize.y);
    }
}
