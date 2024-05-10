using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 2.0f;
    private Vector3 _moveDirection = Vector3.zero;
    private Rigidbody _rb;
    private Animator _animator;
    private Transform _physicalBody;
    private Transform _mainCamera;
    private bool isJumping = false;
    private static readonly int Speed = Animator.StringToHash("Speed");

    void Start()
    {
        InitializeComponents();
    }

    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
        UpdateAnimatorParameters();
    }

    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void InitializeComponents()
    {
        _rb = GetComponent<Rigidbody>();
        _physicalBody = transform.Find("PhysicalBody");
        _animator = _physicalBody.GetComponent<Animator>();
        if (Camera.main != null) _mainCamera = Camera.main.transform;

        if (_physicalBody == null)
        {
            Debug.LogError("Child object 'PhysicalBody' not found");
        }
    }

    private bool IsFalling()
    {
        return _rb.velocity.y < 0 && !isJumping;
    }

    private void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = _mainCamera.forward;
        Vector3 right = _mainCamera.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        _moveDirection = forward * verticalInput + right * horizontalInput;
        _moveDirection *= speed;
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void UpdateAnimatorParameters()
    {
        _animator.SetFloat(Speed, _moveDirection.magnitude);
        _animator.SetBool("Falling", IsFalling());
    }

    private void HandleCollision(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    private void MoveCharacter()
    {
        if (_moveDirection != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + _moveDirection * Time.fixedDeltaTime);
        }
    }
}