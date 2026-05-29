using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    // gridSize is no longer needed since your physical tiles define the grid layout dynamically!
    [SerializeField] int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    // This is called by each Tile script on Start()
    public void RegisterTile(Vector2Int cords, bool isWalkable, float height)
    {
        if (!grid.ContainsKey(cords))
        {
            grid.Add(cords, new Node(cords, isWalkable, height));
        }
        else
        {
            // Update it if it somehow exists already
            grid[cords].isWalkable = isWalkable;
            grid[cords].worldHeight = height;
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

    public float GetTileHeight(Vector2Int targetCords)
    {
        if (grid.ContainsKey(targetCords))
        {
            return grid[targetCords].worldHeight;
        }
        return 0f;
    }
}