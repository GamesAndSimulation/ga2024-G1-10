using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using Project.Internal.Scripts.Enemies.reverse_power;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 700f;
    public float mouseSensitivity = 300f;
    public float jumpForce = 10f;

    [Header("Glide Settings")]
    public KeyCode glideKey = KeyCode.G; // Key to start gliding
    public float glideGravity = 2.0f; // Custom gravity value during glide
    private bool isGliding = false;

    [Header("Terrain Manipulation Settings")]
    public Terrain terrain;

    public float power = 0.2f; // Smaller power for weaker changes
    public float radius = 10.0f;
    public float minRadius = 10.0f; // Minimum radius for manipulation
    public float maxRadius = 15.0f; // Maximum radius for manipulation
    public KeyCode raiseKey = KeyCode.R; // Key to raise the terrain
    public KeyCode lowerKey = KeyCode.F; // Key to lower the terrain

    [Header("Camera Settings")]
    public Transform cameraPivot;
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    [Header("Dimensional Toggle Settings")]
    public KeyCode toggleKey = KeyCode.T;

    [Header("Skybox Settings")]
    public Material originalSkybox; // Reference to the original skybox
    public Material toggledSkybox; // Reference to the toggled skybox

    [Header("Reversal Settings")]
    public KeyCode reversalKey = KeyCode.Z; // Key to activate reversal power

    [Header("Character Settings")]
    private Rigidbody _rigidBody;
    private Transform _physicalBody;
    public Animator animator;
    private bool _isAiming;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private float _rotationX;
    private float _rotationY;

    private bool _isSkyboxToggled = false; // Track whether the skybox is toggled or not
    private readonly Dictionary<Vector2Int, float> _originalHeights = new Dictionary<Vector2Int, float>();
    public AudioSource walkingSound;

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

        // Set the initial skybox
        if (originalSkybox != null)
        {
            RenderSettings.skybox = originalSkybox;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleAiming();
        HandleDimensionalToggle();
        HandleGlide();
        HandleTerrainManipulation();
        HandleReversal();
    }

    public bool IsAiming()
    {
        return _isAiming;
    }

    private void HandleDimensionalToggle()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            DimensionalObjectManager.Instance.ToggleDimensionalObjects();
            ToggleSkybox();
        }
    }

    private void ToggleSkybox()
    {
        if (_isSkyboxToggled)
        {
            RenderSettings.skybox = originalSkybox;
        }
        else
        {
            RenderSettings.skybox = toggledSkybox;
        }
        _isSkyboxToggled = !_isSkyboxToggled;
        DynamicGI.UpdateEnvironment(); // Update the environment lighting
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

    private void HandleAiming()
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

    private void HandleTerrainManipulation()
    {
        if (Input.GetKeyDown(raiseKey))
        {
            ManipulateTerrain(true);
        }

        if (Input.GetKeyDown(lowerKey))
        {
            ManipulateTerrain(false);
        }
    }

    private void ManipulateTerrain(bool raise)
    {
        // Perform a raycast from the camera's position to where the player is looking
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits the terrain
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == terrain.gameObject)
        {
            // Get the hit point in world coordinates
            Vector3 hitPoint = hit.point;

            // Calculate the distance from the player to the hit point
            float distanceFromPlayer = Vector3.Distance(transform.position, hitPoint);

            // Ensure the hit point is within the valid radius range
            if (distanceFromPlayer < minRadius || distanceFromPlayer > maxRadius)
            {
                return;
            }

            // Get the position of the terrain in the world
            Vector3 terrainPosition = terrain.transform.position;

            // Get the TerrainData from the terrain
            TerrainData terrainData = terrain.terrainData;

            // Calculate the base position in the heightmap corresponding to the hit point
            int xBase = (int)((hitPoint.x - terrainPosition.x) / terrainData.size.x * terrainData.heightmapResolution);
            int zBase = (int)((hitPoint.z - terrainPosition.z) / terrainData.size.z * terrainData.heightmapResolution);

            // Calculate the radius in terms of the heightmap resolution
            int radiusInHeights = Mathf.RoundToInt(radius / terrainData.size.x * terrainData.heightmapResolution);

            // Get the current heights in the area around the hit point
            float[,] heights = terrainData.GetHeights(xBase - radiusInHeights / 2, zBase - radiusInHeights / 2, radiusInHeights, radiusInHeights);

            // Loop through the heightmap points in the specified radius
            for (int x = 0; x < radiusInHeights; x++)
            {
                for (int z = 0; z < radiusInHeights; z++)
                {
                    // Calculate the distance from the center of the radius
                    float distance = Vector2.Distance(new Vector2(x, z), new Vector2(radiusInHeights / 2, radiusInHeights / 2));

                    // If the point is within the radius, adjust the height
                    if (distance <= radiusInHeights / 2)
                    {
                        // Calculate the adjustment based on the distance and power
                        float adjustment = (radiusInHeights / 2 - distance) / (radiusInHeights / 2) * power * Time.deltaTime;

                        // Store the original height for future reversion
                        Vector2Int point = new Vector2Int(xBase - radiusInHeights / 2 + x, zBase - radiusInHeights / 2 + z);
                        if (!_originalHeights.ContainsKey(point))
                        {
                            _originalHeights[point] = heights[x, z];
                        }

                        // Raise or lower the terrain based on the input
                        if (raise)
                        {
                            heights[x, z] += adjustment;
                        }
                        else
                        {
                            heights[x, z] -= adjustment;
                        }
                    }
                }
            }

            // Set the new heights back to the terrain
            terrainData.SetHeights(xBase - radiusInHeights / 2, zBase - radiusInHeights / 2, heights);
        }
    }

    public void RevertTerrain()
    {
        // Get the TerrainData from the terrain
        TerrainData terrainData = terrain.terrainData;

        foreach (var entry in _originalHeights)
        {
            int x = entry.Key.x;
            int z = entry.Key.y;
            float[,] heights = terrainData.GetHeights(x, z, 1, 1);
            heights[0, 0] = entry.Value;
            terrainData.SetHeights(x, z, heights);
        }

        // Clear the stored original heights after reverting
        _originalHeights.Clear();
    }

    private void HandleReversal()
    {
        if (Input.GetKeyDown(reversalKey))
        {
            Reversable[] reversableObjects = FindObjectsOfType<Reversable>();
            foreach (Reversable reversable in reversableObjects)
            {
                reversable.StartReversing();
            }
        }
        else if (Input.GetKeyUp(reversalKey))
        {
            Reversable[] reversableObjects = FindObjectsOfType<Reversable>();
            foreach (Reversable reversable in reversableObjects)
            {
                reversable.StopReversing();
            }
        }
    }
}