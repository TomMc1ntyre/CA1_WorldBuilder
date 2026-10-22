using UnityEngine;

// Put this on the mini-map camera. It follows the player from above.
public class MiniMapCamera : MonoBehaviour
{
    public Transform player;
    public float height = 120f;

    [Tooltip("Rotate the map with the player (on) or keep north at the top (off).")]
    public bool rotateWithPlayer = false;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 pos = player.position;
        pos.y = player.position.y + height;
        transform.position = pos;

        float yaw = rotateWithPlayer ? player.eulerAngles.y : 0f;
        transform.rotation = Quaternion.Euler(90f, yaw, 0f);
    }
}
