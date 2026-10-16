using UnityEngine;

// Put this on each coin. The coin needs a Collider with "Is Trigger" ticked.
public class Collectible : MonoBehaviour
{
    public int value = 1;

    [Header("Idle animation")]
    public float spinSpeed = 90f;
    public float bobHeight = 0.25f;
    public float bobSpeed = 2f;

    [Header("Optional feedback")]
    public AudioClip pickupSound;
    public GameObject pickupEffectPrefab;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        if (pickupEffectPrefab) Destroy(Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity), 3f);

        if (GameManager.Instance) GameManager.Instance.AddScore(value);
        Destroy(gameObject);
    }
}
