using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public float speed = 10.0f;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection *= speed;

        if (moveDirection != Vector3.zero)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * Time.deltaTime;
        }
    }
}