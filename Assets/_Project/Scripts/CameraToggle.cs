using UnityEngine;
using UnityEngine.InputSystem;

// Press V (or the key you choose) to switch between third person and first person.
// Put this on any object in the level (e.g. the GameManager).
public class CameraToggle : MonoBehaviour
{
    [Tooltip("The first-person Cinemachine Camera object (keep it switched OFF in the Hierarchy).")]
    public GameObject firstPersonCamera;

    [Tooltip("The player's 'Geometry' object (child of PlayerArmature) - hidden in first person.")]
    public GameObject playerBody;

    [Tooltip("Key that switches the view.")]
    public Key toggleKey = Key.V;

    private Renderer[] bodyRenderers;
    private bool isFirstPerson;

    private void Start()
    {
        if (playerBody) bodyRenderers = playerBody.GetComponentsInChildren<Renderer>(true);
        SetFirstPerson(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            SetFirstPerson(!isFirstPerson);
        }
    }

    private void SetFirstPerson(bool on)
    {
        isFirstPerson = on;

        // Turning the first-person camera on makes Cinemachine switch to it (higher priority)
        if (firstPersonCamera) firstPersonCamera.SetActive(on);

        // Hide the body in first person but keep its shadow
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
}
