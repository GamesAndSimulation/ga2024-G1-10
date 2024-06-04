namespace Project.Internal.Scripts.Enemies.player
{
using UnityEngine;

public class PlayerGlide : MonoBehaviour
{
    public KeyCode glideKey = KeyCode.G; // Key to start gliding
    public float glideGravity = 2.0f; // Custom gravity value during glide
    private bool isGliding = false;
    private Rigidbody _rigidBody;

    void Start()
    {
        if (!TryGetComponent<Rigidbody>(out _rigidBody))
        {
            Debug.LogError("Rigidbody component missing from the player.");
            return;
        }
    }

    void Update()
    {
        HandleGlide();
    }

    private void HandleGlide()
    {
        if (Input.GetButton("Jump") && !IsGrounded() && _rigidBody.velocity.y < 0)
        {
            StartGliding();
        }
        else if (Input.GetButtonUp("Jump") || IsGrounded())
        {
            StopGliding();
        }
    }

    private void StartGliding()
    {
        if (!isGliding)
        {
            isGliding = true;
            _rigidBody.useGravity = false; // Disable default gravity
            Debug.Log("Gliding started");
        }
    }

    private void StopGliding()
    {
        if (isGliding)
        {
            isGliding = false;
            _rigidBody.useGravity = true; // Re-enable default gravity
            Debug.Log("Gliding stopped");
        }
    }

    private void FixedUpdate()
    {
        if (isGliding)
        {
            // Apply custom gravity while gliding
            _rigidBody.AddForce(Vector3.down * glideGravity, ForceMode.Acceleration);
        }
    }

    private bool IsGrounded()
    {
        float rayLength = 1.1f; // Adjust this length as necessary
        Vector3 origin = transform.position + Vector3.up * 0.1f; // Slightly above the player

        // Perform raycasts around the player's base
        bool isGroundedCenter = Physics.Raycast(origin, Vector3.down, rayLength);
        bool isGroundedFront = Physics.Raycast(origin + transform.forward * 0.5f, Vector3.down, rayLength);
        bool isGroundedBack = Physics.Raycast(origin - transform.forward * 0.5f, Vector3.down, rayLength);
        bool isGroundedLeft = Physics.Raycast(origin - transform.right * 0.5f, Vector3.down, rayLength);
        bool isGroundedRight = Physics.Raycast(origin + transform.right * 0.5f, Vector3.down, rayLength);

        return isGroundedCenter || isGroundedFront || isGroundedBack || isGroundedLeft || isGroundedRight;
    }
}

}