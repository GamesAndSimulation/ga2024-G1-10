using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public ParticleSystem particleSystem;
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public GameObject giantBulletPrefab;
    private Animator _animator;
    private Rigidbody _rb;
    private static readonly int CanAttack = Animator.StringToHash("CanAttack");
    private static readonly int IsMovingHorizontally = Animator.StringToHash("IsMovingHorizontally");
    private static readonly int Die = Animator.StringToHash("Die");
    Vector3 direction;
    float distance;
    public float rotationSpeed = 20f;
    private bool isAttacking = false;
    public float attackRange = 5f;
    public float stopRange = 7f;
    private int health = 3;

    public AudioSource bossDeathSound;
    public AudioSource bossDamageSound;
    public AudioSource bossGiantSound;
    public AudioSource bossLiteSound;

    private enum Phase { Phase1, Phase2, Phase3 }
    private Phase currentPhase;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        currentPhase = Phase.Phase1;
        StartCoroutine(PhaseCycle());
    }

    void Update()
    {
        direction = player.position - transform.position;
        distance = direction.magnitude;

        RotateTowardsPlayer(direction);
    }

    void RotateTowardsPlayer(Vector3 direction)
    {
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
    }

    void MoveTowardsPlayer()
    {
        direction.Normalize();
        _rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        _animator.SetBool(IsMovingHorizontally, true);
        _animator.SetBool(CanAttack, false);
    }

    void StopMovingAndShoot()
    {
        _rb.velocity = Vector3.zero;
        _animator.SetBool(IsMovingHorizontally, false);
        _animator.SetBool(CanAttack, true);
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        while (currentPhase == Phase.Phase1 && isAttacking)
        {
            ShootBullet();
            yield return new WaitForSeconds(0.5f);
        }
        isAttacking = false;
    }

    IEnumerator PhaseCycle()
    {
        while (health > 0)
        {
            yield return Phase1Routine();
            yield return Phase2Routine();
            yield return Phase3Routine();
        }
    }

    IEnumerator Phase1Routine()
    {
        float phase1Duration = 10f;
        float phase1StartTime = Time.time;

        while (Time.time - phase1StartTime < phase1Duration)
        {
            if (distance > stopRange)
            {
                MoveTowardsPlayer();
            }
            else if (distance <= attackRange)
            {
                StopMovingAndShoot();
            }
            else
            {
                _animator.SetBool(IsMovingHorizontally, false);
                _animator.SetBool(CanAttack, false);
            }
            yield return null;
        }
        currentPhase = Phase.Phase2;
    }

    void ShootBullet()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2f + transform.up * 2f; // Spawn in front of the enemy
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, transform.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 shootDirection = (player.position - spawnPosition).normalized;
        bulletRb.velocity = shootDirection * 10f; // Adjust bullet speed as needed
        bulletRb.AddForce(transform.forward * 32f, ForceMode.Impulse);
        bossLiteSound.Play();
        //bulletRb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }

    IEnumerator Phase2Routine()
    {
        float phase2Duration = 5f;
        float phase2StartTime = Time.time;

        while (Time.time - phase2StartTime < phase2Duration)
        {
            if (Time.time - phase2StartTime >= 3f)
            {
                ShootGiantBullet();
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(phase2Duration - 3f); // Ensure the total duration is 5 seconds
        currentPhase = Phase.Phase3;
    }

    void ShootGiantBullet()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2f + transform.up * 2f; // Spawn in front of the enemy
        GameObject giantBullet = Instantiate(giantBulletPrefab, spawnPosition, transform.rotation);
        Rigidbody bulletRb = giantBullet.GetComponent<Rigidbody>();
        Vector3 shootDirection = (player.position - spawnPosition).normalized;
        bulletRb.velocity = shootDirection * 5f; // Adjust giant bullet speed as needed
        bossGiantSound.Play();

    }

    IEnumerator Phase3Routine()
    {
        float phase3Duration = 5f;
        float phase3StartTime = Time.time;

        _animator.SetBool(IsMovingHorizontally, false);
        _animator.SetBool(CanAttack, false);
        moveSpeed = 0;

        while (Time.time - phase3StartTime < phase3Duration)
        {
            yield return null;
        }

        moveSpeed = 5f; // Reset to original speed after resting
        currentPhase = Phase.Phase1;
    }

    private IEnumerator PlayParticleSystemForDuration(float duration)
    {
        particleSystem.Play();
        yield return new WaitForSeconds(duration);
        particleSystem.Stop();
    }

    private IEnumerator FinalSceneLoad(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(5);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (currentPhase == Phase.Phase3)
            {
                Bullet bullet = collision.gameObject.GetComponent<Bullet>();
                if (bullet != null)
                {
                    StartCoroutine(PlayParticleSystemForDuration(0.25f));
                    health -= 1;
                    bossDamageSound.Play();
                    if (health <= 0)
                    {
                        _animator.SetBool(Die, true);
                        bossDeathSound.Play();
                        StartCoroutine(FinalSceneLoad(2.0f));
                    }
                }
            }
        }
    }
}
