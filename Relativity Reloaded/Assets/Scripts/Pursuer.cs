using UnityEngine;

namespace Project.Internal.Scripts.Enemies
{
    public class Pursuer : MonoBehaviour
    {
        public Transform player;
        private Animator _animator;
        public float speed = 2f;
        public float rotationSpeed = 200f;
        public float triggerRadius = 10f; // Trigger radius
        public float attackRadius = 2f; // Attack radius

        private Rigidbody _rb;
        private static readonly int Attack = Animator.StringToHash("canAttack");
        private static readonly int IsMovingHorizontally = Animator.StringToHash("isMovingHorizontally");

        private bool isAttacking = false;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();

            if (_animator == null)
            {
                Debug.LogError("Animator component not found on " + gameObject.name);
            }
            if (_rb == null)
            {
                Debug.LogError("Rigidbody component not found on " + gameObject.name);
            }
        }

        void FixedUpdate()
        {
            Vector3 direction = player.position - transform.position;
            float distance = direction.magnitude;

            if (CanAttack(distance))
            {
                _animator.SetBool(Attack, true);
                _animator.SetBool(IsMovingHorizontally, false);
                isAttacking = true;
                RotateTowardsPlayer(direction);
                _rb.velocity = Vector3.zero;
            }
            else if (IsWithinAggro(distance))
            {
                direction.Normalize();
                RotateTowardsPlayer(direction);
                _rb.AddForce(direction * speed, ForceMode.VelocityChange);
                _animator.SetBool(IsMovingHorizontally, true);
                _animator.SetBool(Attack, false);
                isAttacking = false;
            }
            else
            {
                _animator.SetBool(IsMovingHorizontally, false);
                _animator.SetBool(Attack, false);
                isAttacking = false;
                _rb.velocity = Vector3.zero;
            }
        }

        private bool IsWithinAggro(float distance)
        {
            return distance <= triggerRadius;
        }

        private bool CanAttack(float distance)
        {
            return distance <= attackRadius;
        }

        void RotateTowardsPlayer(Vector3 direction)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && isAttacking)
            {
                PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage(1);
                }
            }
        }
    }
}
