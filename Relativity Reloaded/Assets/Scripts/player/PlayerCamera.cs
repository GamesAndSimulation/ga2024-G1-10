namespace Project.Internal.Scripts.Enemies.player
{
using UnityEngine;
using Cinemachine;

public class PlayerCamera
{
    private readonly PlayerMovement _playerMovement;
    private readonly Transform _cameraPivot;
    private readonly CinemachineVirtualCamera _camera1;
    private readonly CinemachineVirtualCamera _camera2;
    private readonly GameObject _crossHair;
    private readonly float _mouseSensitivity;
    public float RotationX { get; set; }
    public float RotationY { get; set; }
    public bool IsFreeCameraActive { get; private set; }

    private Vector3 _cameraLocalPositionBeforeFreeCamera;
    private Quaternion _cameraLocalRotationBeforeFreeCamera;

    public PlayerCamera(PlayerMovement playerMovement, Transform cameraPivot, CinemachineVirtualCamera camera1, CinemachineVirtualCamera camera2, GameObject crossHair, float mouseSensitivity)
    {
        _playerMovement = playerMovement;
        _cameraPivot = cameraPivot;
        _camera1 = camera1;
        _camera2 = camera2;
        _crossHair = crossHair;
        _mouseSensitivity = mouseSensitivity;
    }

    public void ToggleFreeCamera()
    {
        IsFreeCameraActive = !IsFreeCameraActive;

        if (IsFreeCameraActive)
        {
            _cameraLocalPositionBeforeFreeCamera = _cameraPivot.localPosition;
            _cameraLocalRotationBeforeFreeCamera = _cameraPivot.localRotation;
            Cursor.lockState = CursorLockMode.None;
            _crossHair.SetActive(false);
        }
        else
        {
            _cameraPivot.localPosition = _cameraLocalPositionBeforeFreeCamera;
            _cameraPivot.localRotation = _cameraLocalRotationBeforeFreeCamera;
            Cursor.lockState = CursorLockMode.Locked;
            _crossHair.SetActive(true);
        }
    }

    public void HandleFreeCameraMovement()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? _playerMovement.freeCameraRunSpeed : _playerMovement.freeCameraSpeed;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUp = 0f;
        if (Input.GetKey(KeyCode.E)) moveUp = 1f;
        if (Input.GetKey(KeyCode.Q)) moveUp = -1f;

        Vector3 moveDirection = (_cameraPivot.forward * moveVertical + 
                                 _cameraPivot.right * moveHorizontal +
                                 _cameraPivot.up * moveUp).normalized * (currentSpeed * Time.unscaledDeltaTime);

        _cameraPivot.position += moveDirection;

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.unscaledDeltaTime;

        RotationX += mouseX;
        RotationY -= mouseY;
        RotationY = Mathf.Clamp(RotationY, -90f, 90f);

        _cameraPivot.localRotation = Quaternion.Euler(RotationY, RotationX, 0f);
    }
}

}