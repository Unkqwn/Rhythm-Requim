using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] Transform unitTransform;
    [SerializeField] float movementSpeed = 1f;

    GridManager gridManager;

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();

        if (unitTransform == null)
        {
            unitTransform = this.transform;
        }
    }

    void Update()
    {
        if (unitTransform == null || gridManager == null) return;

        Vector2Int gridDirection = Vector2Int.zero;

        // WASD input
        if (Input.GetKeyDown(KeyCode.W)) gridDirection += Vector2Int.up;    // +Y in Grid (Forward in 3D)
        if (Input.GetKeyDown(KeyCode.S)) gridDirection += Vector2Int.down;  // -Y in Grid (Backward in 3D)
        if (Input.GetKeyDown(KeyCode.A)) gridDirection += Vector2Int.left;  // -X in Grid (Left in 3D)
        if (Input.GetKeyDown(KeyCode.D)) gridDirection += Vector2Int.right; // +X in Grid (Right in 3D)

        if (gridDirection != Vector2Int.zero)
        {
            // check de current location
            int currentX = Mathf.RoundToInt(unitTransform.position.x / gridManager.UnityGridSize);
            int currentZ = Mathf.RoundToInt(unitTransform.position.z / gridManager.UnityGridSize);
            Vector2Int currentGridPos = new Vector2Int(currentX, currentZ);

            // check waar de player heen wil
            Vector2Int targetGridPos = currentGridPos + gridDirection;

            // check of walkable
            if (gridManager.IsTileWalkable(targetGridPos))
            {
                unitTransform.position = new Vector3(
                    targetGridPos.x * gridManager.UnityGridSize,
                    unitTransform.position.y,
                    targetGridPos.y * gridManager.UnityGridSize
                );
            }
            else
            {
                Debug.Log("cant move here");
            }
        }
    }
}