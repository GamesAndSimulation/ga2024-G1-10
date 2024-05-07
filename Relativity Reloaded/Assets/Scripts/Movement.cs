using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private Animator animator;
    private Transform physicalBody;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        physicalBody = transform.Find("PhysicalBody");
        animator = physicalBody.GetComponent<Animator>();

        if (physicalBody == null)
        {
            Debug.LogError("Child object 'PhysicalBody' not found");
        }
    }

    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection *= speed;

        // Set the "speed" parameter of the Animator based on the magnitude of the movement direction
        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);
        }
    }
}