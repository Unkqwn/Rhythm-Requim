using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] Transform unitTransform;
    [SerializeField] float movementSpeed = 1f;
    private int lastMovedBeatNumber = -1;

    GridManager gridManager;

    [Header("Extra Settings")]
    [SerializeField] private ParticleSystem beatHitParticles;

    [Header("UI Feedback Settings")]
    [SerializeField] private RectTransform rhythmTargetUI; // Drag your center rhythm UI element here
    [SerializeField] private float beatScaleBump = 1.4f;     // How big it pops when you hit it right
    [SerializeField] private float scaleShrinkSpeed = 10f;  // How fast it shrinks back down

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

        if (rhythmTargetUI != null)
        {
            rhythmTargetUI.localScale = Vector3.Lerp(
                rhythmTargetUI.localScale,
                Vector3.one,
                scaleShrinkSpeed * Time.deltaTime
            );
        }

        bool inputPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) ||
                             Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D);

        if (inputPressed)
        {
            int currentBeatInteger = Mathf.RoundToInt(Conductor.instance.SongPositionInBeats);

            if (currentBeatInteger == lastMovedBeatNumber) return; // spam block

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

                        Vector3 lookDirection = new Vector3(gridDirection.x, 0f, gridDirection.y);
                        if (lookDirection != Vector3.zero)
                        {
                            unitTransform.rotation = Quaternion.LookRotation(lookDirection);
                        }

                        lastMovedBeatNumber = currentBeatInteger;

                        if (beatHitParticles != null) beatHitParticles.Play();

                        if (rhythmTargetUI != null)
                        {
                            rhythmTargetUI.localScale = Vector3.one * beatScaleBump;
                        }

                        if (Camera.main != null)
                        {
                            CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
                            if (camFollow != null)
                            {
                                camFollow.TriggerHitKick();
                            }
                        }

                        Debug.Log($"hit beat #{currentBeatInteger}!");
                    }
                }
            }
            else
            {
                Debug.LogWarning("Missed the beat");
            }
        }
    }
}