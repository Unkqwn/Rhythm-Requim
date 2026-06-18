using UnityEngine;

public class Conductor : MonoBehaviour
{
    [Header("Beat Settings")]
    [SerializeField] private float bpm = 120f;
    [SerializeField] private float firstBeatOffset;
    private float beatInterval;
    private float dspSongTime;
    private float songPositionInBeats;
    private int lastBeat = 0;

    [Header("Note Settings")]
    [SerializeField] private GameObject beatVisual;
    [SerializeField] private float beatsToArrive = 5f;
    [SerializeField] private float beatDistance = 300f;

    [Header("Input Window Settings")]
    [SerializeField] private float timingWindowSeconds = 0.2f;

    public static Conductor instance;

    void Awake() { instance = this; }

    public float SongPositionInBeats => songPositionInBeats;
    public float BeatInterval => beatInterval;

    private void Start()
    {
        beatInterval = 60f / bpm;
        dspSongTime = (float)AudioSettings.dspTime;
        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        songPositionInBeats = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset) / beatInterval;

        int currentBeat = Mathf.FloorToInt(songPositionInBeats);
        if (currentBeat != lastBeat)
        {
            lastBeat = currentBeat;
            SpawnNote();
        }
    }

    public bool IsOnBeat()
    {
        // Find the nearest whole number beat (e.g., if songPositionInBeats is 4.15, nearest is 4.0)
        float nearestBeat = Mathf.Round(songPositionInBeats);

        // Calculate the difference in fractional beats
        float beatDifference = Mathf.Abs(songPositionInBeats - nearestBeat);

        // Convert that beat fraction back into raw audio seconds
        float timeDifferenceInSeconds = beatDifference * beatInterval;

        // Return true if the player clicked within the 0.2s window
        return timeDifferenceInSeconds <= timingWindowSeconds;
    }

    private void SpawnNote()
    {
        float travelTime = beatInterval * beatsToArrive;

        Vector3 leftSpawn = transform.position + new Vector3(-beatDistance, 0, 0);
        Vector3 rightSpawn = transform.position + new Vector3(beatDistance, 0, 0);

        Instantiate(beatVisual, leftSpawn, Quaternion.identity, transform)
            .AddComponent<BeatNote>().Init(leftSpawn, transform.position, travelTime);
        Instantiate(beatVisual, rightSpawn, Quaternion.identity, transform)
            .AddComponent<BeatNote>().Init(rightSpawn, transform.position, travelTime);
    }
}