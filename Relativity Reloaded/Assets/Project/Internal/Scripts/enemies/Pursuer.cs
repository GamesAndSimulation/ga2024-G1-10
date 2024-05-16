using UnityEngine;

public class Pursuer : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    public float speed = 2f;
    public float rotationSpeed = 200f;
    public float triggerRadius = 10f; // Trigger radius
    public float attackRadius = 1f; // Attack radius



    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 direction = player.position - transform.position;

        if (direction.magnitude <= triggerRadius && !isPlayerAttackable(direction))
        {
            direction.Normalize();

            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);

            transform.position += direction * speed * Time.deltaTime;

            animator.SetBool("isPlayerAttackable", false);
            Debug.Log("Player is not attackable");
        }
        else if (isPlayerAttackable(direction))
        {
            animator.SetBool("isPlayerAttackable", true);
            Debug.Log("Player is attackable");
        }

    }

    bool isPlayerAttackable(Vector3 distance)
    {
        return distance.magnitude <= attackRadius;
    }

}