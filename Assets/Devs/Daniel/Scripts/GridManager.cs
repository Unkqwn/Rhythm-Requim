using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int unityGridSize;
    public int UnityGridSize { get { return unityGridSize; } }

    // FIX: Initialize the dictionary directly inline right here!
    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();

    // Delete the private void Awake() method entirely if it's still there!

    public void RegisterTile(Vector2Int cords, bool isWalkable, float height)
    {
        // Now 'grid' will never be null when tiles call this
        if (!grid.ContainsKey(cords))
        {
            grid.Add(cords, new Node(cords, isWalkable, height));
        }
        else
        {
            grid[cords].isWalkable = isWalkable;
            grid[cords].worldHeight = height;
        }
    }

    public bool IsTileWalkable(Vector2Int targetCords)
    {
        if (grid != null && grid.ContainsKey(targetCords))
        {
            return grid[targetCords].isWalkable;
        }
        return false;
    }

    public float GetTileHeight(Vector2Int targetCords)
    {
        if (grid != null && grid.ContainsKey(targetCords))
        {
            return grid[targetCords].worldHeight;
        }
        return 0f;
    }
}