using UnityEngine;

public class Playershoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // The bullet prefab to instantiate
    public Transform bulletSpawnPoint;  // The point from where the bullet will be spawned
    public float bulletSpeed = 20f;  // The speed of the bullet

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Add velocity to the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
    }
}
