using UnityEngine;

public class BeatNote : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private float travelTime;
    private float elapsed = 0f;

    public void Init(Vector3 from, Vector3 to, float duration)
    {
        startPos = from;
        targetPos = to;
        travelTime = duration;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        float t = elapsed / travelTime;

        transform.position = Vector3.Lerp(startPos, targetPos, t);

        if (t >= 1f)
            Destroy(gameObject);
    }
}