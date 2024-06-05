using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public GameObject bulletPrefab;
    public Camera playerCamera;
    public Vector3 shootOffset = new Vector3(0, 2.5f, 0f);
    public Transform playerTransform;
    public PlayerMovement playerMovement;
    public AudioSource shootMusic;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerMovement.IsAiming())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 spawnPosition = playerTransform.position + playerTransform.TransformDirection(shootOffset);
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Bullet prefab does not have a Rigidbody component.");
            return;
        }

        Vector3 shootDirection = playerCamera.transform.forward;
        rb.velocity = shootDirection * bulletSpeed;
        shootMusic.Play();
    }
}
