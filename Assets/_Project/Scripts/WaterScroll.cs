using UnityEngine;

// Animates water by scrolling the normal map on the water material.
// Put this on the water plane. Works with a URP/Lit material that has a Normal Map.
public class WaterScroll : MonoBehaviour
{
    [Tooltip("How fast the ripples move (X and Y).")]
    public Vector2 scrollSpeed = new Vector2(0.02f, 0.015f);

    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        // URP Lit uses the Base Map's tiling/offset for the normal map too
        mat.SetTextureOffset("_BaseMap", offset);
    }
}
