namespace Project.Internal.Scripts.Enemies.mandatory_features
{
    using UnityEngine;

    public class FreeCameraController : MonoBehaviour
    {
        public float movementSpeed = 10.0f;
        public float lookSpeed = 2.0f;
        private bool isActive = false;

        void Update()
        {
            if (!isActive) return;

            // Camera rotation
            float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);

            // Camera movement
            float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
            float moveSide = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
            transform.Translate(new Vector3(moveSide, 0, moveForward));
        }

        public void SetActive(bool active)
        {
            isActive = active;
            if (isActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

}