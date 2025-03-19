using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveDistance = 1f; // Distance to move per key press
    public float xRange = 5f; // Horizontal movement boundaries
    public float horizontalSpeed = 5f; // Speed of horizontal movement

    [Header("Car Specifications")]
    public float motorForce = 15f; // Increased from 5f for more noticeable movement
    public float brakeForce = 3f;
    public float maxSpeed = 100f;
    public float reverseMaxSpeed = 30f;

    // Private variables
    private Rigidbody rb;
    private float accelerationInput;
    private float horizontalInput;
    private float currentBrakeForce;
    private bool isReversing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Make sure rigidbody is properly configured
        if (rb != null)
        {
            // Check for frozen position constraints
            if ((rb.constraints & RigidbodyConstraints.FreezePositionZ) != 0)
            {
                // Unfreeze Z-axis movement if it was frozen
                rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                Debug.Log("Unfroze Z-axis constraint to allow forward movement");
            }

            // Set appropriate drag values
            rb.linearDamping = 0.5f;
        }
        else
        {
            Debug.LogError("No Rigidbody component found! Please add a Rigidbody to this GameObject.");
        }
    }

    void Update()
    {
        // Get input
        GetInput();

        // Debug output for troubleshooting
        Debug.Log($"Acceleration: {accelerationInput}, Speed: {rb.linearVelocity.magnitude}");
    }

    private void FixedUpdate()
    {
        // Apply forward/backward movement first
        HandleMotor();

        // Apply horizontal movement
        HandleHorizontalMovement();

        // Apply boundaries
        ApplyBoundaries();

        // Limit speed
        LimitSpeed();
    }

    void GetInput()
    {
        // Get horizontal input
        horizontalInput = 0f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = -1f;
        }

        // Get acceleration and braking input
        accelerationInput = 0f;
        currentBrakeForce = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            accelerationInput = 1f;
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            float forwardVelocity = Vector3.Dot(rb.linearVelocity, transform.forward);
            if (forwardVelocity > 0.5f)
            {
                // Apply brakes when moving forward
                currentBrakeForce = brakeForce;
                accelerationInput = 0f;
            }
            else
            {
                // Apply reverse throttle when stopped or moving slowly
                accelerationInput = -1f;
                isReversing = true;
            }
        }
        else
        {
            isReversing = false;
        }
    }

    private void HandleHorizontalMovement()
    {
        if (horizontalInput != 0)
        {
            // Calculate desired horizontal velocity
            Vector3 horizontalVelocity = Vector3.right * horizontalInput * horizontalSpeed;

            // Only apply horizontal velocity (keep current Y and Z velocity)
            Vector3 currentVelocity = rb.linearVelocity;
            Vector3 targetVelocity = new Vector3(horizontalVelocity.x, currentVelocity.y, currentVelocity.z);

            // Apply horizontal movement using forces
            Vector3 velocityChange = targetVelocity - currentVelocity;
            velocityChange.y = 0;
            velocityChange.z = 0;

            rb.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    private void HandleMotor()
    {
        // Direct application of force for debugging - this ensures the car moves forward
        if (accelerationInput != 0)
        {
            // Apply a direct force in the forward direction
            Vector3 forceDirection = accelerationInput > 0 ? transform.forward : -transform.forward;
            float forceMagnitude = accelerationInput > 0 ? motorForce : (motorForce * 0.5f);

            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Force);

            // Debug visualization
            Debug.DrawRay(transform.position, forceDirection * 2, Color.red, 0.1f);
        }

        // Apply brakes if needed
        if (currentBrakeForce > 0)
        {
            // Apply brakes
            rb.AddForce(-rb.linearVelocity.normalized * currentBrakeForce);
        }
    }

    void ApplyBoundaries()
    {
        // Clamp position within boundaries
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, -xRange, xRange);
        transform.position = position;
    }

    private void LimitSpeed()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        float maxAllowedSpeed = isReversing ? reverseMaxSpeed : maxSpeed;

        if (currentSpeed > maxAllowedSpeed)
        {
            float brakeSpeed = currentSpeed - maxAllowedSpeed;
            Vector3 normalizedVelocity = rb.linearVelocity.normalized;
            Vector3 brakeVelocity = normalizedVelocity * brakeSpeed;

            rb.AddForce(-brakeVelocity * 2); // Apply limiting force
        }
    }
}