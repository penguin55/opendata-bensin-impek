using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RoomManager : MonoBehaviour
{
    //Note, yang ini belum kuoptimize dan masih berantakan. Nanti kuberesin
    public static RoomManager Instance;

    [SerializeField] private Vector2 spaceScale;
    [SerializeField] private Vector2Int roomSize;
    [SerializeField] private RoomTemplate roomTemplate;

    [SerializeField] private Room roomPrefabs;
    [SerializeField] private Transform parent;
    private MapLayout layout;

    private Vector3 scaleParent = new Vector3(1, -1, 1);

    private void Start()
    {
        Instance = this;

        layout = new MapLayout();
        layout.Init(roomSize.x, roomSize.y);
        layout.GenerateLayout();

        SpawningRoom();
    }

    private void SpawningRoom()
    {
        int[,] grid = layout.GetGridLayout();

        for (int i = 0; i < roomSize.y; i++)
        {
            for (int j = 0; j < roomSize.x; j++)
            {
                if (grid[j,i] != 0)
                {
                    Vector2 spawnPos = new Vector2(j, i) * spaceScale;
                    Room spawn = Instantiate(roomPrefabs, spawnPos, Quaternion.identity, parent);
                    spawn.Init(grid, j, i, grid[j,i]);
                    spawn.transform.localScale = spaceScale;

                    if (spawn.IsEnemyArea())
                    {
                        ScanRoomPath(spawnPos * scaleParent);
                    }

                    if ((j, i) == layout.GetStartNode()) spawn.SetStateRoom("START");
                    else if ((j, i) == layout.GetEndNode()) spawn.SetStateRoom("END");
                }
            }
        }

        parent.localScale = scaleParent;
        if (AstarPath.active.graphs.Length > 0)
        {
            AstarPath.active.Scan();
        }
    }

    private void ScanRoomPath(Vector2 position)
    {
        // This holds all graph data
        AstarData data = AstarPath.active.data;

        // This creates a Grid Graph
        GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;

        // Setup a grid graph with some values
        int width = 31;
        int depth = 31;
        float nodeSize = 1;

        gg.center = position;

        gg.rotation = new Vector3(90, 0, 0);

        gg.collision.heightCheck = false;

        gg.collision.mask = LayerMask.GetMask("Obstacle");
        // Updates internal size from the above values
        gg.SetDimensions(width, depth, nodeSize);
        gg.collision.use2D = true;
        gg.collision.collisionCheck = true;
        gg.collision.type = ColliderType.Ray;

        /*// Scans all graphs
        AstarPath.active.Scan();*/
    }

    public (int x, int y) RoomSize()
    {
        return (roomSize.x , roomSize.y);
    }
}
