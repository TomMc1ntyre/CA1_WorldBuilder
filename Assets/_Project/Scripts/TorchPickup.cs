using UnityEngine;

// Put this on the torch item placed in the house.
// The item needs a Collider with "Is Trigger" ticked (e.g. a Sphere Collider, radius ~1).
public class TorchPickup : MonoBehaviour
{
    [Tooltip("The PlayerTorch component (on the GameManager).")]
    public PlayerTorch playerTorch;

    public AudioClip pickupSound;

    [Header("Idle animation")]
    public float spinSpeed = 60f;
    public float bobHeight = 0.15f;
    public float bobSpeed = 2f;

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
        if (!other.CompareTag("Player") || playerTorch == null) return;

        playerTorch.GiveTorch();
        if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Destroy(gameObject);
    }
}
