using UnityEngine;

namespace Project.Internal.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        public float runSpeed = 10f; // Running speed
        public float rotationSpeed = 700f; // Speed of rotation
        public float mouseSensitivity = 100f; // Sensitivity of mouse movement for rotation
        public Animator animator; // Reference to the animator
        public Transform cameraTransform; // Reference to the camera transform
        private Rigidbody _rigidBody; // Reference to the Rigidbody
        private Transform _physicalBody; // Reference to the PhysicalBody
        private static readonly int Speed = Animator.StringToHash("Speed");

        private float _rotationX; // Rotation around the y-axis
        public Transform PlayerTarget;

        void Start()
        {
            // Retrieve the Rigidbody and PhysicalBody
            _rigidBody = GetComponent<Rigidbody>();
            _physicalBody = transform.Find("PhysicalBody");
        }

        void FixedUpdate()
        {
            // Check if the left shift key is being pressed
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            // Use runSpeed if the player is running, otherwise use speed
            float currentSpeed = isRunning ? runSpeed : speed;

            // Move the player forward and backward
            float moveVertical = Input.GetAxis("Vertical");
            // Move the player left and right
            float moveHorizontal = Input.GetAxis("Horizontal");

            // Get the camera's forward and right vectors
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Project forward and right vectors onto the horizontal plane (y = 0)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Combine the movement direction based on camera's orientation
            Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized * (currentSpeed * Time.deltaTime);

            // Apply the movement
            _rigidBody.MovePosition(_rigidBody.position + moveDirection);

            // Rotate the PhysicalBody to face the move direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                _physicalBody.rotation = Quaternion.Slerp(_physicalBody.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            // Update the animator with the speed
            if (moveDirection == Vector3.zero)
            {
                animator.SetFloat(Speed, 0f);
            }
            else if (!isRunning)
            {
                animator.SetFloat(Speed, 0.5f);
            }
            else
            {
                animator.SetFloat(Speed, 1f);
            }

            // Handle mouse input for rotation
            if (Input.GetMouseButton(0)) // If the left mouse button is held down
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                _rotationX += mouseX;
                
                // Apply rotation to the player target
                PlayerTarget.localRotation = Quaternion.Euler(0f, _rotationX, 0f);
            }

            // Handle jump input
            if (Input.GetButtonDown("Jump") && _rigidBody.velocity.y == 0)
            {
                _rigidBody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            }
        }
    }
}
