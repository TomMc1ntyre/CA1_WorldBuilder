using UnityEngine;

// Put this on PlayerArmature (the object that has the Animator).
// IMPORTANT: in the Animator Controller, the Base Layer must have "IK Pass" ticked.
// Once the torch has been picked up, the right arm is held out in front of the robot,
// pointing the way the BODY faces (not the camera), while the rest of the body animates normally.
[RequireComponent(typeof(Animator))]
public class HoldTorchIK : MonoBehaviour
{
    [Tooltip("The PlayerTorch component (on the GameManager).")]
    public PlayerTorch playerTorch;

    [Header("Where the hand is held (relative to the robot's chest)")]
    [Tooltip("x = right, y = up, z = forward (metres)")]
    public Vector3 handOffset = new Vector3(0.25f, -0.2f, 0.45f);

    [Tooltip("How much the torch tilts down towards the ground (degrees). 0 = straight ahead.")]
    [Range(0f, 45f)] public float downwardTilt = 12f;

    [Tooltip("How strongly the arm is held in place (1 = fully)")]
    [Range(0f, 1f)] public float holdWeight = 1f;

    [Tooltip("Extra rotation of the hand so the torch points forward. Adjust if the torch points the wrong way.")]
    public Vector3 handRotationOffset = new Vector3(0f, 0f, 0f);

    [Tooltip("How fast the arm raises / lowers")]
    public float blendSpeed = 4f;

    private Animator animator;
    private float currentWeight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        bool holding = playerTorch != null && playerTorch.HasTorch;
        currentWeight = Mathf.MoveTowards(currentWeight, holding ? holdWeight : 0f, blendSpeed * Time.deltaTime);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, currentWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, currentWeight);
        if (currentWeight <= 0f) return;

        // Use the direction the robot's body is facing
        Vector3 bodyForward = transform.forward;
        bodyForward.y = 0f;
        bodyForward.Normalize();
        Vector3 right = transform.right;

        // Tilt the aim slightly down so the beam lights the ground ahead
        Vector3 aimForward = Quaternion.AngleAxis(downwardTilt, right) * bodyForward;

        Transform chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        Vector3 basePos = chest != null ? chest.position : transform.position + Vector3.up * 1.3f;

        Vector3 handPos = basePos
                          + right * handOffset.x
                          + Vector3.up * handOffset.y
                          + bodyForward * handOffset.z;

        Quaternion handRot = Quaternion.LookRotation(aimForward, Vector3.up) * Quaternion.Euler(handRotationOffset);

        animator.SetIKPosition(AvatarIKGoal.RightHand, handPos);
        animator.SetIKRotation(AvatarIKGoal.RightHand, handRot);
    }
}
