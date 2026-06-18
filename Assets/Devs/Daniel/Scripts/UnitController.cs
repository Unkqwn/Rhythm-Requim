using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] Transform unitTransform;
    [SerializeField] float movementSpeed = 1f;
    private int lastMovedBeatNumber = -1;

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
        if (unitTransform == null || gridManager == null || Conductor.instance == null) return;

        // Detect if ANY movement key was tapped this frame
        bool inputPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                             Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D);

        if (inputPressed)
        {
            // Get the current beat integer from the conductor (e.g. Beat 4, Beat 5...)
            int currentBeatInteger = Mathf.RoundToInt(Conductor.instance.SongPositionInBeats);

            // ?? CHECK A: Have we already moved on this exact beat number?
            if (currentBeatInteger == lastMovedBeatNumber)
            {
                Debug.LogWarning("?? Already moved on this beat! Wait for the next one.");
                return; // Reject spam instantly
            }

            // ?? CHECK B: Is the player clicking within the timing window?
            if (Conductor.instance.IsOnBeat())
            {
                Vector2Int gridDirection = Vector2Int.zero;

                if (Input.GetKeyDown(KeyCode.W)) gridDirection += Vector2Int.up;
                if (Input.GetKeyDown(KeyCode.S)) gridDirection += Vector2Int.down;
                if (Input.GetKeyDown(KeyCode.A)) gridDirection += Vector2Int.left;
                if (Input.GetKeyDown(KeyCode.D)) gridDirection += Vector2Int.right;

                if (gridDirection != Vector2Int.zero)
                {
                    int currentX = Mathf.RoundToInt(unitTransform.position.x / gridManager.UnityGridSize);
                    int currentZ = Mathf.RoundToInt(unitTransform.position.z / gridManager.UnityGridSize);
                    Vector2Int currentGridPos = new Vector2Int(currentX, currentZ);

                    Vector2Int targetGridPos = currentGridPos + gridDirection;

                    if (gridManager.IsTileWalkable(targetGridPos))
                    {
                        float targetHeight = gridManager.GetTileHeight(targetGridPos);

                        unitTransform.position = new Vector3(
                            targetGridPos.x * gridManager.UnityGridSize,
                            targetHeight,
                            targetGridPos.y * gridManager.UnityGridSize
                        );

                        // Rotate character toward walking direction
                        Vector3 lookDirection = new Vector3(gridDirection.x, 0f, gridDirection.y);
                        if (lookDirection != Vector3.zero)
                        {
                            unitTransform.rotation = Quaternion.LookRotation(lookDirection);
                        }

                        // ?? SUCCESS LOCK: Save the current beat number so we can't move here again!
                        lastMovedBeatNumber = currentBeatInteger;

                        Debug.Log($"On beat #{currentBeatInteger}!");
                    }
                }
            }
            else
            {
                // Penalty block: Misclicked the window entirely
                Debug.LogWarning("Missed beat");
            }
        }
    }
}