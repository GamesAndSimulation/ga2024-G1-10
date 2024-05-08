using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 _moveDirection = Vector3.zero;
    private Rigidbody _rb;
    private Animator _animator;
    private Transform _physicalBody;
    private Transform _mainCamera;
    private static readonly int Speed = Animator.StringToHash("Speed");

    void Start()
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

    void Update()
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

        // Set the "speed" parameter of the Animator based on the magnitude of the movement direction
        _animator.SetFloat(Speed, _moveDirection.magnitude);
    }

    void FixedUpdate()
    {
        if (_moveDirection != Vector3.zero)
        {
            _rb.MovePosition(_rb.position + _moveDirection * Time.fixedDeltaTime);
        }
    }
}