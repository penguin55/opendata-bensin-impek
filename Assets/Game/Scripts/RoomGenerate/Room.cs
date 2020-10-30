using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    [SerializeField] private bool enemyArea;

    private (int x, int y) position;
    private int room_id;

    public void Init(int[,] grid, int x, int y, int room_id)
    {
        position = (x, y);
        this.room_id = room_id;

        CheckRoomAround(grid);
    }

    public void SetStateRoom(string state)
    {
        if (state.Equals("START")) GetComponent<SpriteRenderer>().color = Color.yellow;
        else if (state.Equals("END")) GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void CheckRoomAround(int[,] grid)
    {
        (int x, int y) gridSize = RoomManager.Instance.RoomSize();

        //Top check
        if (IsValueOnRange(position.y + 1, 0, gridSize.y))
        {
            int diff = Mathf.Abs(grid[position.x, position.y + 1] - room_id);
            if (grid[position.x, position.y + 1] != 0 && diff <= 1) doors[0].SetActive(true);
        }

        // Right check
        if (IsValueOnRange(position.x + 1, 0, gridSize.x))
        {
            int diff = Mathf.Abs(grid[position.x + 1, position.y] - room_id);
            if (grid[position.x + 1, position.y] != 0 && diff <= 1) doors[1].SetActive(true);
        }

        //Bottom check
        if (IsValueOnRange(position.y - 1, 0, gridSize.y))
        {
            int diff = Mathf.Abs(grid[position.x, position.y - 1] - room_id);
            if (grid[position.x, position.y - 1] != 0 && diff <= 1) doors[2].SetActive(true);
        }

        // Left check
        if (IsValueOnRange(position.x - 1, 0, gridSize.x))
        {
            int diff = Mathf.Abs(grid[position.x - 1, position.y] - room_id);
            if (grid[position.x - 1, position.y] != 0 && diff <= 1) doors[3].SetActive(true);
        }
    }

    private bool IsValueOnRange(int valueCheck, int minBoundInclude, int maxBoundExclude)
    {
        return valueCheck >= minBoundInclude && valueCheck < maxBoundExclude;
    }

    public bool IsEnemyArea()
    {
        return enemyArea;
    }

}
