using UnityEngine;
using UnityEngine.InputSystem;

// Put this on the GameManager (or any object that is always active).
// The torch light is a Spot Light parented to PlayerCameraRoot (inside PlayerArmature), so it
// comes from the player and points where the player is looking (first and third person).
public class PlayerTorch : MonoBehaviour
{
    [Tooltip("Spot Light that is a child of the Main Camera. Keep it switched OFF.")]
    public Light torchLight;

    [Tooltip("Key to switch the torch on/off once it has been picked up.")]
    public Key toggleKey = Key.F;

    [Tooltip("Optional click sound when switching the torch.")]
    public AudioClip clickSound;

    [Tooltip("Torch model placed in the robot's hand (child of the Right_Hand bone). Keep it switched OFF.")]
    public GameObject heldTorchModel;

    public bool HasTorch { get; private set; }

    private void Start()
    {
        if (torchLight) torchLight.enabled = false;
        if (heldTorchModel) heldTorchModel.SetActive(false);
    }

    private void Update()
    {
        if (!HasTorch || torchLight == null || Keyboard.current == null) return;

        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            torchLight.enabled = !torchLight.enabled;
            if (clickSound) AudioSource.PlayClipAtPoint(clickSound, torchLight.transform.position, 0.6f);
        }
    }

    // Called by TorchPickup when the player collects the torch
    public void GiveTorch()
    {
        HasTorch = true;
        if (torchLight) torchLight.enabled = true;   // switch it on straight away
        if (heldTorchModel) heldTorchModel.SetActive(true); // show the torch in the robot's hand
        if (GameManager.Instance)
            GameManager.Instance.ShowMessage("Torch equipped! Press " + toggleKey + " to switch it on/off", 3f);
    }
}
