using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float runSpeed = 10f;
    public float freeCameraSpeed = 7f; // Speed for free camera movement
    public float freeCameraRunSpeed = 14f; // Run speed for free camera movement
    public float rotationSpeed = 700f;
    public float mouseSensitivity = 300f;
    public float jumpForce = 10f;

    [Header("Camera Settings")]
    public Transform cameraPivot;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("UI Settings")]
    public GameObject crossHair; // Reference to the CrossHair GameObject

    [Header("Character Settings")]
    private Rigidbody _rigidBody;
    private Transform _physicalBody;
    public Animator animator;
    private bool _isAiming;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private float _rotationX;
    private float _rotationY;

    [FormerlySerializedAs("_isSkyboxToggled")]
    [SerializeField]
    private bool isSkyboxToggled; // Track whether the skybox is toggled or not

    public readonly Dictionary<Vector2Int, float> OriginalHeights = new Dictionary<Vector2Int, float>();
    public AudioSource walkingSound;

    private PlayerStats _playerStats;
    private bool _isFreeCameraActive = false;
    private bool _isGamePaused = false;
    private Vector3 _cameraLocalPositionBeforeFreeCamera;
    private Quaternion _cameraLocalRotationBeforeFreeCamera;

    private List<Animator> animators = new List<Animator>();
    private List<Rigidbody> rigidbodies = new List<Rigidbody>();

    void Start()
    {
        if (!TryGetComponent(out _rigidBody))
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
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        crossHair.SetActive(true);

        _playerStats = GetComponent<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats component missing from the player.");
        }

        FindAllAnimatorsAndRigidbodies();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleFreeCamera();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePauseGame();
        }

        if (_isFreeCameraActive)
        {
            HandleFreeCameraMovement();
        }
        else if (!_isGamePaused)
        {
            HandleMovement();
            HandleRotation();
            HandleJump();
            HandleAiming();
        }
    }

    private void FindAllAnimatorsAndRigidbodies()
    {
        animators.AddRange(FindObjectsOfType<Animator>());
        rigidbodies.AddRange(FindObjectsOfType<Rigidbody>());
    }

    private void ToggleFreeCamera()
    {
        _isFreeCameraActive = !_isFreeCameraActive;

        if (_isFreeCameraActive)
        {
            // Store camera's local position and rotation before switching to free camera
            _cameraLocalPositionBeforeFreeCamera = cameraPivot.localPosition;
            _cameraLocalRotationBeforeFreeCamera = cameraPivot.localRotation;
            Cursor.lockState = CursorLockMode.None;
            crossHair.SetActive(false); // Disable CrossHair in free camera mode
        }
        else
        {
            // Reset camera local position and rotation to its original state
            cameraPivot.localPosition = _cameraLocalPositionBeforeFreeCamera;
            cameraPivot.localRotation = _cameraLocalRotationBeforeFreeCamera;
            Cursor.lockState = CursorLockMode.Locked;
            crossHair.SetActive(true); // Enable CrossHair when exiting free camera mode
        }
    }

    private void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;

        if (_isGamePaused)
        {
            foreach (var animator in animators)
            {
                animator.enabled = false;
            }
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            foreach (var animator in animators)
            {
                animator.enabled = true;
            }
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;
            }
        }
    }

    private void HandleFreeCameraMovement()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? freeCameraRunSpeed : freeCameraSpeed;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = 0f;
        if (Input.GetKey(KeyCode.E)) moveUp = 1f;
        if (Input.GetKey(KeyCode.Q)) moveUp = -1f;

        Vector3 moveDirection = (cameraPivot.forward * moveVertical + 
                                 cameraPivot.right * moveHorizontal +
                                 cameraPivot.up * moveUp).normalized * (currentSpeed * Time.unscaledDeltaTime);

        cameraPivot.position += moveDirection;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

        _rotationX += mouseX;
        _rotationY -= mouseY;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(_rotationY, _rotationX, 0f);
    }

    public bool IsAiming()
    {
        return _isAiming;
    }

    private void HandleMovement()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : speed;

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized *
                                (currentSpeed * Time.deltaTime);

        _rigidBody.MovePosition(_rigidBody.position + moveDirection);

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _physicalBody.rotation =
                Quaternion.Slerp(_physicalBody.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        animator.SetFloat(Speed, moveDirection == Vector3.zero ? 0f : (isRunning ? 1f : 0.5f));
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _rotationX += mouseX;
        _rotationY -= mouseY;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(_rotationY, _rotationX, 0f);

        if (_isAiming)
        {
            Vector3 cameraForward = cameraPivot.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            if (cameraForward != Vector3.zero)
            {
                _physicalBody.forward = cameraForward;
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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

    private void HandleAiming()
    {
        if (_playerStats != null && _playerStats.HasGun)
        {
            _isAiming = Input.GetMouseButton(1); // Right mouse button

            if (_isAiming)
            {
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(true);
            }
            else
            {
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(false);
            }

            Debug.Log($"Aiming status: {_isAiming}");
        }
        else
        {
            _isAiming = false;
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
        }
    }
}
