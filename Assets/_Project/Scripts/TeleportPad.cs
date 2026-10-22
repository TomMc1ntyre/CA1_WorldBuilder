using UnityEngine;

// Put this on a teleport pad that has a Collider with "Is Trigger" ticked.
// Drag the pad you want to arrive at into "Destination".
// Works with the Starter Assets third person controller (CharacterController).
public class TeleportPad : MonoBehaviour
{
    [Tooltip("Where the player arrives (usually another pad).")]
    public Transform destination;

    [Tooltip("How high above the destination the player appears.")]
    public float arriveHeight = 0.5f;

    [Tooltip("Seconds before the player can teleport again (stops ping-ponging).")]
    public float cooldown = 1.5f;

    [Header("Optional feedback")]
    public AudioClip teleportSound;
    public ParticleSystem departEffect;
    public ParticleSystem arriveEffect;

    private static float lastTeleportTime = -10f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || destination == null) return;
        if (Time.time - lastTeleportTime < cooldown) return;
        lastTeleportTime = Time.time;

        if (departEffect) departEffect.Play();
        if (teleportSound) AudioSource.PlayClipAtPoint(teleportSound, transform.position);

        // CharacterController overrides position changes unless it is switched off first
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;

        other.transform.position = destination.position + Vector3.up * arriveHeight;
        other.transform.rotation = Quaternion.Euler(0f, destination.eulerAngles.y, 0f);

        if (cc) cc.enabled = true;

        if (arriveEffect) arriveEffect.Play();
    }

    // Draws a line to the destination in the Scene view so pairs are easy to see
    private void OnDrawGizmos()
    {
        if (destination == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, destination.position);
        Gizmos.DrawWireSphere(destination.position, 0.5f);
    }
}
