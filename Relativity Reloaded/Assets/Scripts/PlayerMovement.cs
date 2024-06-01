using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")] 
    public float speed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 700f;
    public float mouseSensitivity = 300f;
    public float jumpForce = 10f;

    [Header("Camera Settings")] 
    public Transform cameraPivot;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("Dimensional Toggle Settings")]
    public KeyCode toggleKey = KeyCode.T;

    [Header("Character Settings")] 
    private Rigidbody _rigidBody;
    private Transform _physicalBody;
    public Animator animator;
    private bool isAiming;

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
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleAiming();
        HandleDimensionalToggle();
    }

    private void HandleDimensionalToggle()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            DimensionalObjectManager.Instance.ToggleDimensionalObjects();
        }
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

        if (isAiming)
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
        if (Input.GetButtonDown("Jump") && Mathf.Approximately(_rigidBody.velocity.y, 0))
        {
            _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleAiming()
    {
        isAiming = Input.GetMouseButton(1); // Right mouse button

        if (isAiming)
        {
            camera1.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
        }
        else
        {
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
        }

        Debug.Log($"Aiming status: {isAiming}");
    }

    public bool IsAiming()
    {
        return isAiming;
    }
}
