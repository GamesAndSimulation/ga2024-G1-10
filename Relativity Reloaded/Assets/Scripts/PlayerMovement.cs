using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using Project.Internal.Scripts.Enemies.player;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float runSpeed = 10f;
    public float freeCameraSpeed = 7f;
    public float freeCameraRunSpeed = 14f;
    public float rotationSpeed = 700f;
    public float mouseSensitivity = 300f;
    public float jumpForce = 10f;

    [Header("Camera Settings")]
    public Transform cameraPivot;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("UI Settings")]
    public GameObject crossHair;

    [Header("Character Settings")]
    public Animator animator;
    public AudioSource walkingSound;

    private Rigidbody _rigidBody;
    private Transform _physicalBody;
    private PlayerStats _playerStats;
    private PlayerCamera _playerCamera;
    private PlayerInput _playerInput;
    private PlayerPause _playerPause;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private bool _isAiming;
    private float _rotationX;
    private float _rotationY;
    private bool _modulesInitialized = false;

    private void Start()
    {
        InitializeComponents();
        InitializeModules();
        
        Debug.Log("CREATION");        

        _playerInput.OnToggleFreeCamera += _playerCamera.ToggleFreeCamera;
        _playerInput.OnTogglePauseGame += _playerPause.TogglePauseGame;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "FinalBossLevel" && !_modulesInitialized)
        {
            InitializeModules();
            _modulesInitialized = true;
        }
        
        _playerInput.CheckInput();

        if (_playerCamera.IsFreeCameraActive)
        {
            _playerCamera.HandleFreeCameraMovement();
        }
        else if (!_playerPause.IsGamePaused)
        {
            HandleMovement();
            HandleRotation();
            HandleJump();
            HandleAiming();
        }
    }
    
    public bool IsAiming()
    {
        return _isAiming;
    }

    private void InitializeComponents()
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

        _playerStats = GetComponent<PlayerStats>();
        if (_playerStats == null)
        {
            Debug.LogError("PlayerStats component missing from the player.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        crossHair.SetActive(true);
    }

    private void InitializeModules()
    {
        _playerCamera = new PlayerCamera(this, cameraPivot, camera1, camera2, crossHair, mouseSensitivity);
        _playerInput = new PlayerInput();
        _playerPause = new PlayerPause();
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

        Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized * (currentSpeed * Time.deltaTime);

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
        float rayLength = 1.1f;
        Vector3 origin = transform.position + Vector3.up * 0.1f;

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
            _isAiming = Input.GetMouseButton(1);

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
        }
        else
        {
            Debug.Log("player_stats: " + _playerStats);
            Debug.Log("player_stats.HasGun: " + _playerStats.HasGun);
            
            _isAiming = false;
            camera1.gameObject.SetActive(true);
            camera2.gameObject.SetActive(false);
        }
    }
}
