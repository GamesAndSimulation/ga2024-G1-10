using UnityEngine;

namespace Project.Internal.Scripts.enemies
{
    public class Pursuer : MonoBehaviour
    {
        public Transform player;
        public Animator animator;

        public float speed = 2f;
        public float rotationSpeed = 200f;
        public float triggerRadius = 10f; // Trigger radius
        private float attackRadius = 1f; // Attack radius



        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (distance <= attackRadius)
            {
                // Can attack
                animator.SetBool("canAttack", true);
                animator.SetBool("isMovingHorizontally", false);
                // Stop movement
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else if (distance <= triggerRadius)
            {
                // Can move towards player
                direction.Normalize();

                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);

                transform.position += direction * speed * Time.deltaTime;

                animator.SetBool("isMovingHorizontally", true);
                animator.SetBool("canAttack", false);
            }
            else
            {
                // Player is out of range
                animator.SetBool("isMovingHorizontally", false);
                animator.SetBool("canAttack", false);
            }
        }

        bool canAttack(Vector3 distance)
        {
            return distance.magnitude <= attackRadius;
        }

    }
}