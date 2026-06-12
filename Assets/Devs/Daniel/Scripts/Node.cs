using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates;
    public bool isWalkable;
    public float worldHeight;

    public Node(Vector2Int coordinates, bool isWalkable, float worldHeight = 0f)
    {
        this.coordinates = coordinates;
        this.isWalkable = isWalkable;
        this.worldHeight = worldHeight;
    }
}