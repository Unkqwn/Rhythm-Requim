using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;

    [Header("Position Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 7f, -10f);
    [SerializeField] private float smoothSpeed = 5f;

    [Header("Angle Settings")]
    [SerializeField] private float pitchAngle = 35f;

    [Header("Camera Juice Settings")]
    [SerializeField] private Vector3 kickVector = new Vector3(0f, 1f, 1f);
    [SerializeField] private float kickReturnSpeed = 8f;

    [Header("FOV Juice Settings")]
    [SerializeField] private float defaultFOV = 60f;       // Normal gameplay FOV
    [SerializeField] private float hitFOVBump = 68f;        // What it pops to on a hit (higher = zoom out distortion)
    [SerializeField] private float fovReturnSpeed = 10f;    // How fast it returns to default

    private Vector3 currentKickOffset = Vector3.zero;
    private Camera cam;

    void Start()
    {
        transform.rotation = Quaternion.Euler(pitchAngle, 0f, 0f);

        // Grab the camera component attached to this GameObject
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            cam.fieldOfView = defaultFOV;
        }
    }

    void Update()
    {
        // 1. Smoothly fade the position kick back to zero
        currentKickOffset = Vector3.Lerp(currentKickOffset, Vector3.zero, kickReturnSpeed * Time.deltaTime);

        // 2. Smoothly glide the FOV back to its default value
        if (cam != null)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, fovReturnSpeed * Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset + currentKickOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
    public void TriggerHitKick()
    {
        // Instantly kick the position forward
        currentKickOffset = kickVector;

        // Instantly pop the FOV wide open
        if (cam != null)
        {
            cam.fieldOfView = hitFOVBump;
        }
    }
}