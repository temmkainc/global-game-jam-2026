using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;

    [SerializeField]
    Transform eyePoint; // The target the camera follows (place this on the player's head)

    public float sensitivity = 2;
    public float smoothing = 1.5f;

    [Header("Camera Smoothing")]
    public float positionSmoothTime = 0.05f;

    Vector2 velocity;
    Vector2 frameVelocity;
    Vector3 positionVelocity = Vector3.zero;

    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>()?.transform;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Snap camera to eye point immediately on start
        if (eyePoint != null)
        {
            transform.position = eyePoint.position;
        }
    }

    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate character horizontally (yaw)
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);

        // Build the target rotation for the camera (yaw + pitch)
        Quaternion targetRotation = Quaternion.AngleAxis(velocity.x, Vector3.up)
                                  * Quaternion.AngleAxis(-velocity.y, Vector3.right);

        transform.rotation = targetRotation;
    }

    void LateUpdate()
    {
        if (eyePoint == null) return;

        // Smoothly follow the eye point position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            eyePoint.position,
            ref positionVelocity,
            positionSmoothTime
        );
    }
}