using UnityEngine;
using Cinemachine;

namespace Project.Internal.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 5f;
        public float runSpeed = 10f;
        public float rotationSpeed = 700f;
        public float mouseSensitivity = 100f;
        public Animator animator;
        public Transform cameraTransform;
        public Transform cameraPivot;
        public CinemachineVirtualCamera camera1;
        public CinemachineVirtualCamera camera2;
        private Rigidbody _rigidBody;
        private Transform _physicalBody;
        private StarterAssets.StarterAssetsInputs _input;
        private bool _cameraSwitch = false;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private float _rotationX;
        private float _rotationY;

        void Start()
        {
            if (!TryGetComponent<Rigidbody>(out _rigidBody))
            {
                Debug.LogError("Rigidbody component missing from the player.");
                return;
            }

            _physicalBody = transform.Find("PhysicalBody");
            if (_physicalBody == null)
            {
                Debug.LogError("PhysicalBody transform missing from the player.");
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;

            if (!TryGetComponent<StarterAssets.StarterAssetsInputs>(out _input))
            {
                Debug.LogError("StarterAssetsInputs component missing from the player.");
            }
        }

        void FixedUpdate()
        {
            if (_input == null) return;

            Debug.Log("FixedUpdate called");
            HandleMovement();
            HandleRotation();
            HandleJump();
            HandleCameraSwitch();
        }

        private void HandleMovement()
        {
            bool isRunning = _input.sprint;
            float currentSpeed = isRunning ? runSpeed : speed;

            float moveVertical = _input.move.y;
            float moveHorizontal = _input.move.x;

            Debug.Log($"Move Input: {moveHorizontal}, {moveVertical}");

            Vector3 forward = cameraPivot.forward;
            Vector3 right = cameraPivot.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized * (currentSpeed * Time.deltaTime);

            Debug.Log($"Move Direction: {moveDirection}");

            _rigidBody.MovePosition(_rigidBody.position + moveDirection);

            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                _physicalBody.rotation = Quaternion.Slerp(_physicalBody.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }

            animator.SetFloat(Speed, moveDirection == Vector3.zero ? 0f : (isRunning ? 1f : 0.5f));
        }

        private void HandleRotation()
        {
            float mouseX = _input.look.x * mouseSensitivity * Time.deltaTime;
            float mouseY = _input.look.y * mouseSensitivity * Time.deltaTime;

            Debug.Log($"Look Input: {mouseX}, {mouseY}");

            _rotationX += mouseX;
            _rotationY -= mouseY;
            _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

            cameraPivot.localRotation = Quaternion.Euler(_rotationY, _rotationX, 0f);
        }

        private void HandleJump()
        {
            if (_input.jump && Mathf.Approximately(_rigidBody.velocity.y, 0))
            {
                _rigidBody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            }
        }

        private void HandleCameraSwitch()
        {
            if (_input.switchCamera)
            {
                _cameraSwitch = !_cameraSwitch;
                camera1.gameObject.SetActive(!_cameraSwitch);
                camera2.gameObject.SetActive(_cameraSwitch);
                Debug.Log($"Camera switched: {_cameraSwitch}");
            }
        }
    }
}
