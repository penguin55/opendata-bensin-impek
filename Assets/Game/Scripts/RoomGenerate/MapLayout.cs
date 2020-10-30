using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLayout
{
    private Vector2 mapSize;
    private int[,] gridMap;

    private Queue<MapData> queue;

    private int generateRoom;

    private (int x, int y) startNode, endNode;
    private (int x, int y) currentNode;

    private int currentRoomId = 1;

    private bool isGeneratingLayout;

    public void Init(int width, int height)
    {
        queue = new Queue<MapData>();
        mapSize = new Vector2(width, height);
        gridMap = new int[width, height];
        currentRoomId = 1;

        generateRoom = (int) Random.Range(4, mapSize.x * mapSize.y);
        currentNode = (0, 0);
        Debug.Log(generateRoom);
    }

    public void GenerateLayout()
    {
        startNode = currentNode;
        gridMap[startNode.x, startNode.y] = currentRoomId;
        queue.Enqueue(new MapData(currentNode.x, currentNode.y));
        isGeneratingLayout = true;

        for (int i = 0; i < generateRoom; i++)
        {
            if (!isGeneratingLayout) break;

            SpawnRoom();
            DecidingToGenerate();
        }
    }

    public (int x, int y) GetStartNode()
    {
        return startNode;
    }

    public (int x, int y) GetEndNode()
    {
        return endNode;
    }

    public int[,] GetGridLayout()
    {
        return gridMap;
    }

    private void DecidingToGenerate()
    {
        string neighbourSpawnableCode = GetNeighbourSpawnableCode(currentNode.x, currentNode.y);

        if (neighbourSpawnableCode.Replace("0", "").Length > 0)
        {
            float randomBehaviour = Random.Range(0f, 1f);

            if (randomBehaviour >= 0.5f) NextQueue();
        } else NextQueue();
    }

    private void NextQueue()
    {
        queue.Dequeue();    
        if (queue.Count > 0)
        {
            currentNode = queue.Peek().position;
            currentRoomId = gridMap[currentNode.x, currentNode.y];
        } else
        {
            isGeneratingLayout = false;
        }
    }

    private void SpawnRoom()
    {
        string neighbourSpawnableCode = GetNeighbourSpawnableCode(currentNode.x, currentNode.y);

        string normalizeNeighbourCode = neighbourSpawnableCode.Replace("0","");

        int spawnSided = normalizeNeighbourCode.Length;

        if (spawnSided > 1)
        {
            int randomSpawn = Random.Range(0, spawnSided);
            string roomSpawned = normalizeNeighbourCode[randomSpawn].ToString();
            SpawningRoom(roomSpawned);
        } else if (spawnSided == 1)
        {
            string roomSpawned = normalizeNeighbourCode[0].ToString();
            SpawningRoom(roomSpawned);
        }
    }

    private void SpawningRoom(string roomSpawned)
    {
        switch (roomSpawned)
        {
            case "1":
                queue.Enqueue(new MapData(currentNode.x, currentNode.y + 1));
                gridMap[currentNode.x, currentNode.y + 1] = currentRoomId + 1;
                endNode = (currentNode.x, currentNode.y + 1);
                break;
            case "2":
                queue.Enqueue(new MapData(currentNode.x + 1, currentNode.y));
                gridMap[currentNode.x + 1, currentNode.y] = currentRoomId + 1;
                endNode = (currentNode.x + 1, currentNode.y);
                break;
            case "3":
                queue.Enqueue(new MapData(currentNode.x, currentNode.y - 1));
                gridMap[currentNode.x, currentNode.y - 1] = currentRoomId + 1;
                endNode = (currentNode.x, currentNode.y - 1);
                break;
            case "4":
                queue.Enqueue(new MapData(currentNode.x - 1, currentNode.y));
                gridMap[currentNode.x - 1, currentNode.y] = currentRoomId + 1;
                endNode = (currentNode.x - 1, currentNode.y);
                break;
        }
    }

    private string GetNeighbourSpawnableCode(int originX, int originY)
    {
        int topNeighbour = IsAvailableToSpawn(originX, originY + 1) ? 1 : 0;
        int rightNeighbour = IsAvailableToSpawn(originX + 1, originY) ? 2 : 0;
        int bottomNeighbour = IsAvailableToSpawn(originX, originY - 1) ? 3 : 0;
        int leftNeighbour = IsAvailableToSpawn(originX - 1, originY) ? 4 : 0;

        return topNeighbour.ToString() + rightNeighbour.ToString() + bottomNeighbour.ToString() + leftNeighbour.ToString(); 
    }

    private bool IsAvailableToSpawn(int neighbourX, int neighbourY)
    {
        bool insideMapX = neighbourX >= 0 && neighbourX < mapSize.x;
        bool insideMapY = neighbourY >= 0 && neighbourY < mapSize.y;

        if (insideMapX && insideMapY)
        {
            bool spaceOccupied = gridMap[neighbourX, neighbourY] != 0;
            return !spaceOccupied;
        }
        else return false;
    }
}

[System.Serializable]
public class MapData
{
    public (int x, int y) position;

    public MapData(int x, int y)
    {
        position.x = x;
        position.y = y;
    }
}
