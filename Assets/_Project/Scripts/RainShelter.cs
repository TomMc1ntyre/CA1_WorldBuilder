using UnityEngine;

// Put this on an invisible box (Box Collider with Is Trigger ticked) that covers
// a sheltered area: inside the house, the secret vault, under a dense group of trees.
// When the player walks in, the rain stops; when they leave every shelter, it starts again.
public class RainShelter : MonoBehaviour
{
    [Tooltip("The Rain particle system (the one parented to the player).")]
    public ParticleSystem rain;

    // Counts how many shelters the player is inside, so overlapping zones work correctly
    private static int sheltersInside = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || rain == null) return;
        sheltersInside++;
        var emission = rain.emission;
        emission.enabled = false;   // stop new drops; existing ones finish falling
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || rain == null) return;
        sheltersInside = Mathf.Max(0, sheltersInside - 1);
        if (sheltersInside == 0)
        {
            var emission = rain.emission;
            emission.enabled = true;
        }
    }

    // Reset the counter when a scene loads (e.g. restarting the level)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetCounter()
    {
        sheltersInside = 0;
    }

    // Shows the shelter area as a blue box in the Scene view
    private void OnDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        if (box == null) return;
        Gizmos.color = new Color(0.3f, 0.6f, 1f, 0.25f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(box.center, box.size);
    }
}
