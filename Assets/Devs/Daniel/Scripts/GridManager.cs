using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] int unityGridSize;

    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    Dictionary<Vector2Int, Node> Grid {  get { return grid; } }

    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int cords = new Vector2Int(x, y);
                grid.Add(cords, new Node(cords, true));

                //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //Vector3 position = new Vector3(cords.x * unitGridSize, 0f, cords.y * unitGridSize);
                //cube.transform.position = position;
                //cube.transform.SetParent(transform);
            }
        }
    }

    public bool IsTileWalkable(Vector2Int targetCords)
    {
        if (grid.ContainsKey(targetCords))
        {
            return grid[targetCords].isWalkable;
        }

        return false;
    }
}
