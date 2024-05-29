using System.Collections;
using UnityEngine;


    public class AlienShootAI : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private GameObject bulletPrefab; // Bullet prefab to be instantiated
        [SerializeField] private Transform bulletSpawnPoint; // Point where the bullet is instantiated
        [SerializeField] private float speed = 2f;
        [SerializeField] private float rotationSpeed = 200f;
        [SerializeField] private float triggerRadius = 10f; // Trigger radius
        [SerializeField] private float attackRadius = 2f; // Attack radius
        [SerializeField] private float shootingInterval = 1f; // Interval between shots

        private Animator _animator;
        private Rigidbody _rb;
        private static readonly int Attack = Animator.StringToHash("canAttack");
        private static readonly int IsMovingHorizontally = Animator.StringToHash("isMovingHorizontally");

        private bool isAttacking = false;
        private bool isShooting = false;

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

                if (!isShooting)
                {
                    StartCoroutine(Shoot());
                }
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

    private IEnumerator Shoot()
        {
            isShooting = true;

            while (IsWithinAggro((player.position - transform.position).magnitude))
            {
                Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                yield return new WaitForSeconds(shootingInterval);
            }

            isShooting = false;
        }
    }

