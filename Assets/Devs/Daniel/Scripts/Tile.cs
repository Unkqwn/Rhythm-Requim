using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] bool isWalkable = true;

    void Start()
    {
        // 1. Grab coordinates from the Labeller script already on this tile
        Labeller labeller = GetComponent<Labeller>();
        if (labeller == null)
        {
            Debug.LogError($"Tile at {transform.position} is missing its Labeller script!");
            return;
        }

        Vector2Int cords = labeller.cords;

        // 2. Grab its actual Y position from the 3D scene
        float physicalHeight = transform.position.y;

        // 3. Register this tile directly into the GridManager dictionary
        GridManager gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager != null)
        {
            gridManager.RegisterTile(cords, isWalkable, physicalHeight);
        }
    }
}