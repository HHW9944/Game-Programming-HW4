using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSensitivity = 2f;
    public GameObject bullet1Prefab;
    public GameObject bullet2Prefab;
    public GameObject enemyPrefab;
    public float baseBulletSpeed = 5f;
    public float bulletSpeed;
    public float baseEnemySpeed = 2f;
    public float enemySpeed;
    public float baseEnemyRespawnTime = 1f;
    public float EnemyRespawnTime;
    public static int enemyDestroyedCount = 0;
    public static int bullet02Count = 0;
    public Transform bulletSpawnPoint;
    public Transform[] spawnPoints;

    private Rigidbody playerRigidbody;

    void Start()
    {
        Debug.Log("Game Start");
        playerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(SpawnEnemies());

        bulletSpeed = baseBulletSpeed;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        HandleShooting();
        AdjustBulletSpeed();
    }

    void AdjustBulletSpeed()
    {
        bulletSpeed = baseBulletSpeed + (enemyDestroyedCount / 5) * 2f;
        enemySpeed = baseEnemySpeed + (enemyDestroyedCount / 5) * 2f;
        EnemyRespawnTime = baseEnemyRespawnTime / (enemyDestroyedCount / 5 + 1);
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement);
        playerRigidbody.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 playerRotation = new Vector3(0, mouseX * lookSensitivity, 0);
        transform.Rotate(playerRotation);

        Camera.main.transform.Rotate(-mouseY * lookSensitivity, 0, 0);
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnemyRespawnTime);
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = spawnPoint.forward * enemySpeed;
            }
            Destroy(enemy, 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bulletSpeed = baseBulletSpeed * 1.2f;
            Shoot(bullet1Prefab);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if(bullet02Count > 0)
            {
                bullet02Count--;
                bulletSpeed = baseBulletSpeed * 3f;
                Shoot(bullet2Prefab);
            }
        }
    }

    void Shoot(GameObject bulletPrefab)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
            Destroy(bullet, 2.5f);
        }
    }
}
