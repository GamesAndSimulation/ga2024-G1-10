using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")] public float speed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 700f;
    public float mouseSensitivity = 300f;
    public float jumpForce = 10f;

    
    [Header("Camera Settings")] public Transform cameraPivot;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("Dimensional Toggle Settings")]
    public KeyCode toggleKey = KeyCode.T;

    public float fadeDuration = 1.0f;

    [Header("Character Settings")] private Rigidbody _rigidBody;
    private Transform _physicalBody;
    public Animator animator;
    private List<GameObject> dimensionalObjects = new List<GameObject>();
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

        dimensionalObjects = DimensionalObjectManager.Instance.GetDimensionalObjects();
        Debug.Log($"Found {dimensionalObjects.Count} dimensional objects.");
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleAiming();
        HandleRockToggle(); // Add this line to call the new method
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

    private void HandleRockToggle()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            foreach (GameObject rock in dimensionalObjects)
            {
                if (rock != null)
                {
                    if (rock.activeSelf)
                    {
                        StartCoroutine(FadeAndToggle(rock, fadeDuration, false));
                    }
                    else
                    {
                        StartCoroutine(FadeAndToggle(rock, fadeDuration, true));
                    }
                }
            }
        }
    }

    private IEnumerator FadeAndToggle(GameObject dimensionalObject, float duration, bool fadeIn)
    {
        Renderer renderer = dimensionalObject.GetComponent<Renderer>();
        if (renderer == null)
        {
            yield break;
        }

        Material material = renderer.material;
        Color initialColor = material.color;
        float elapsedTime = 0f;

        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        if (fadeIn)
        {
            dimensionalObject.SetActive(true);
        }

        // Set material to use fade mode
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        if (!fadeIn)
        {
            dimensionalObject.SetActive(false);
        }
    }
}