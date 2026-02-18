using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementBunnyHop : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxGroundSpeed = 10f;
    public float groundAccel = 80f;
    public float airAccel = 20f;
    public float jumpForce = 7f;
    public float friction = 6f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private PlayerInput pi;

    private Vector2 moveInput;
    private bool jumpInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        pi = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        pi.actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        pi.actions["Move"].canceled += ctx => moveInput = Vector2.zero;
        pi.actions["Jump"].performed += ctx => jumpInput = true;
    }

    void OnDisable()
    {
        pi.actions["Move"].performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        pi.actions["Move"].canceled -= ctx => moveInput = Vector2.zero;
        pi.actions["Jump"].performed -= ctx => jumpInput = true;
    }

    void FixedUpdate()
    {
        Vector3 wishDir = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        Vector3 velocity = rb.linearVelocity;
        Vector3 horizontalVel = new Vector3(velocity.x, 0, velocity.z);

        if (isGrounded)
        {
            // Apply friction when grounded and not moving
            horizontalVel = ApplyFriction(horizontalVel);

            // Ground acceleration (capped)
            horizontalVel = Accelerate(horizontalVel, wishDir, groundAccel, maxGroundSpeed);

            // Jump
            if (jumpInput)
            {
                velocity.y = jumpForce;
                jumpInput = false;
                isGrounded = false;
            }
        }
        else
        {
            // Air acceleration (does not reset max speed)
            horizontalVel = AirAccelerate(horizontalVel, wishDir, airAccel);
        }

        rb.linearVelocity = new Vector3(horizontalVel.x, velocity.y, horizontalVel.z);
    }

    Vector3 ApplyFriction(Vector3 vel)
    {
        if (moveInput.magnitude > 0.1f) return vel; // skip friction if moving

        float speed = vel.magnitude;
        if (speed < 0.001f) return Vector3.zero;

        float drop = speed * friction * Time.fixedDeltaTime;
        float newSpeed = Mathf.Max(speed - drop, 0);
        return vel * (newSpeed / speed);
    }

    Vector3 Accelerate(Vector3 currentVel, Vector3 wishDir, float accel, float maxSpeed)
    {
        if (wishDir == Vector3.zero)
            return currentVel;

        float projSpeed = Vector3.Dot(currentVel, wishDir);
        float addSpeed = maxSpeed - projSpeed;
        if (addSpeed <= 0) return currentVel;

        float accelSpeed = accel * Time.fixedDeltaTime;
        if (accelSpeed > addSpeed) accelSpeed = addSpeed;

        currentVel += wishDir * accelSpeed;
        return currentVel;
    }

    Vector3 AirAccelerate(Vector3 currentVel, Vector3 wishDir, float accel)
    {
        if (wishDir == Vector3.zero)
            return currentVel;

        // Project current speed onto wishDir (Quake-style)
        float projSpeed = Vector3.Dot(currentVel, wishDir);
        float accelSpeed = accel * Time.fixedDeltaTime;

        // Allow buildup in air, but control it
        if (projSpeed + accelSpeed > projSpeed)
            currentVel += wishDir * accelSpeed;

        return currentVel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
            isGrounded = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask) != 0)
            isGrounded = false;
    }
}
