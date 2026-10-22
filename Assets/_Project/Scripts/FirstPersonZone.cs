using UnityEngine;

// Put this on an invisible trigger box covering the INSIDE of the house
// (Box Collider with "Is Trigger" ticked).
// When the player walks in, a first-person camera takes over and the player's body is hidden
// (it still casts a shadow). When they walk out, the normal third-person camera returns.
public class FirstPersonZone : MonoBehaviour
{
    [Tooltip("The first-person Cinemachine Camera object (keep it switched OFF in the Hierarchy).")]
    public GameObject firstPersonCamera;

    [Tooltip("The player's 'Geometry' object (child of PlayerArmature) - hidden while in first person.")]
    public GameObject playerBody;

    private Renderer[] bodyRenderers;

    private void Start()
    {
        if (firstPersonCamera) firstPersonCamera.SetActive(false);
        if (playerBody) bodyRenderers = playerBody.GetComponentsInChildren<Renderer>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        SetFirstPerson(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        SetFirstPerson(false);
    }

    private void SetFirstPerson(bool on)
    {
        // Turning the first-person camera on makes Cinemachine switch to it (higher priority)
        if (firstPersonCamera) firstPersonCamera.SetActive(on);

        // Hide the body so the camera isn't looking at the inside of the robot's head,
        // but keep its shadow so it still feels grounded
        if (bodyRenderers != null)
        {
            foreach (Renderer r in bodyRenderers)
            {
                r.shadowCastingMode = on
                    ? UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly
                    : UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
    }

    // Shows the zone as an orange box in the Scene view
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;
        Gizmos.color = new Color(1f, 0.6f, 0.2f, 0.25f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(box.center, box.size);
    }
}
