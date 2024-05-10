using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public GameObject bulletPrefab;
    public float maxClickDuration = 0.2f; // Maximum time in seconds between mouse down and up
    public float shootAngle = 45f; // Angle in degrees to shoot the bullet

    private float mouseDownTime;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownTime = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - mouseDownTime <= maxClickDuration)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, transform.localScale.y / 2, 0) + transform.forward;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
    
        Vector3 shootDirection = Quaternion.Euler(-shootAngle, 0, 0) * transform.forward;
        rb.velocity = shootDirection * bulletSpeed;
    }
}