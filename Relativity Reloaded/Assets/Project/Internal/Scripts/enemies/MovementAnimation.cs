using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
    }

    void Update()
    {
        float horizontalSpeed = GetComponent<Rigidbody>().velocity.x;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isMoving = Mathf.Abs(horizontalSpeed) > 0;
        bool isPlayerAttackable = animator.GetBool("isPlayerAttackable");
        bool isNotAttacking = !stateInfo.IsName("Attack");
        bool isNotIdling = !stateInfo.IsName("Idle");

        if (isMoving && !isPlayerAttackable && isNotAttacking)
        {
            animator.Play("Walk");
        }
        else if (isPlayerAttackable && isNotAttacking && isNotIdling)
        {
            animator.Play("Attack");
        }
        else if (!isMoving && !isPlayerAttackable)
        {
            animator.Play("Idle");
        }
    }
}