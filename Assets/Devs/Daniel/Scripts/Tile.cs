using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isWalkable = true;

    void Awake()
    {
        GridManager gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null) return;

        // Calculate grid coordinates automatically from physical 3D space positions!
        int gridX = Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize);
        int gridZ = Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize);
        Vector2Int cords = new Vector2Int(gridX, gridZ);

        float physicalHeight = transform.position.y;

        gridManager.RegisterTile(cords, isWalkable, physicalHeight);
    }
}