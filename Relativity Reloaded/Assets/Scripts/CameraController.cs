using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 1.0f;
    private Vector3 offset;
    private Transform _transform;

    void Start()
    {
        _transform = transform;
        offset = _transform.position - target.position;
        offset.y += 5;
        //offset.z *= 0.5f; // Reduce the z offset by 50%
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            mouseX = Mathf.Clamp(mouseX, -35, 60);

            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
            mouseY = Mathf.Clamp(mouseY, -35, 60);

            _transform.RotateAround(target.position, Vector3.up, mouseX);
            _transform.RotateAround(target.position, _transform.right, -mouseY);

            // Rotate the target around the y-axis
            target.Rotate(0, mouseX, 0);

            Quaternion rotationX = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion rotationY = Quaternion.AngleAxis(-mouseY, _transform.right);

            offset = rotationX * rotationY * offset;
        }

        Vector3 desiredPosition = target.position + offset;
        _transform.position = desiredPosition; // Directly assign the desired position

        _transform.LookAt(target);
    }
}