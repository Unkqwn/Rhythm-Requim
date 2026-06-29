using UnityEngine;

public class FighterJetBoss : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject jetLaserPrefab;
    [SerializeField] private Transform leftMuzzle;
    [SerializeField] private Transform rightMuzzle;

    [Header("Grid Config")]
    [SerializeField] private float unityGridSize = 2f;
    [SerializeField] private int strafeLaneLength = 12; // How far down the grid the jet targets

    [Header("Jet Movement Settings")]
    [SerializeField] private float flyToLaneSpeed = 15f; // Speed of banking into position

    private int lastBeatProcessed = -1;
    private int bossActionCycle = 0;
    private float targetWorldX = 0f;
    private int targetLaneX = 0;

    void Start()
    {
        targetWorldX = transform.position.x;
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Lerp(currentPos.x, targetWorldX, flyToLaneSpeed * Time.deltaTime);
        transform.position = currentPos;

        if (Conductor.instance == null || playerTransform == null) return;

        // Sync to the music engine
        int currentBeat = Mathf.FloorToInt(Conductor.instance.SongPositionInBeats);

        if (currentBeat != lastBeatProcessed)
        {
            lastBeatProcessed = currentBeat;
            ProcessBossBeat(currentBeat);
        }
    }

    private void ProcessBossBeat(int beatCount)
    {
        int phase = bossActionCycle % 4;
        bossActionCycle++;

        switch (phase)
        {
            case 0:
                LockOnPlayerLane();
                break;

            case 1:
                HighlightTargetLane(true);
                Debug.LogWarning(" JET WEAPONS LOCK: LANE " + targetLaneX);
                break;

            case 2:
                HighlightTargetLane(false);
                FireGuns();
                break;

            case 3:
                Debug.Log("Jet cooling weapon systems.");
                break;
        }
    }

    private void LockOnPlayerLane()
    {
        // Identify the player's current column
        targetLaneX = Mathf.RoundToInt(playerTransform.position.x / unityGridSize);

        // Update the target X position so the update loop smoothly glides the jet there
        targetWorldX = targetLaneX * unityGridSize;
    }

    private void HighlightTargetLane(bool activate)
    {
        // Target forward out from the nose of the jet down the vertical track lanes
        for (int i = 1; i <= strafeLaneLength; i++)
        {
            // Vector3.forward assumes your jet is parked at the bottom looking up, 
            // or modify to Vector3.back if it is at the top looking down!
            Vector3 targetWorldPos = transform.position + (Vector3.forward * (i * unityGridSize));

            Collider[] floorObjects = Physics.OverlapSphere(targetWorldPos, 0.5f);
            foreach (Collider col in floorObjects)
            {
                MeshRenderer tileMesh = col.GetComponent<MeshRenderer>();
                if (tileMesh != null && col.CompareTag("Tile"))
                {
                    // Turns the tiles bright laser red for warning feedback
                    tileMesh.material.color = activate ? Color.red : Color.white;
                }
            }
        }
    }

    private void FireGuns()
    {
        if (jetLaserPrefab == null) return;

        // Fire a double shot from the wings/muzzles of the fighter jet!
        if (leftMuzzle != null) SpawnLaser(leftMuzzle.position);
        if (rightMuzzle != null) SpawnLaser(rightMuzzle.position);
    }

    private void SpawnLaser(Vector3 spawnPosition)
    {
        GameObject projectile = Instantiate(jetLaserPrefab, spawnPosition, Quaternion.identity);
        PlayerBullet projectileScript = projectile.GetComponent<PlayerBullet>();

        if (projectileScript != null)
        {
            // Send the laser screaming forward down the lane
            projectileScript.Setup(Vector3.forward, 30f);
        }
    }
}