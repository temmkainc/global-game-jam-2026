using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    [SerializeField] private Animator _animator;
    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    private Rigidbody _rigidbody;
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    public bool isLocked = false;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isLocked)
        {
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _animator.SetFloat("Speed", 0f);
            return;
        }
        IsRunning = canRun && Input.GetKey(runningKey);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;

        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        Vector2 input = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        float inputMagnitude = Mathf.Clamp01(input.magnitude);

        Vector3 velocity = transform.rotation *
            new Vector3(input.x * targetMovingSpeed, _rigidbody.linearVelocity.y, input.y * targetMovingSpeed);

        _rigidbody.linearVelocity = velocity;

        float animationSpeed = inputMagnitude * (targetMovingSpeed / runSpeed);
        _animator.SetFloat("Speed", animationSpeed);
    }

}