using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float fireRate = 2f;
    public float projectileDelay = 1.0f; // Delay between projectiles

    private void Start()
    {
        StartCoroutine(FireProjectilesWithDelay());
    }

    IEnumerator FireProjectilesWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(projectileDelay);
            FireProjectile();
        }
    }

    void Update()
    {
        if (player == null)
        {
            // Player is not assigned, do nothing
            return;
        }

        // Move towards the player in the X and Y axes only
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.z = 0f; // Ignore the Z-axis
        transform.Translate(directionToPlayer.normalized * moveSpeed * Time.deltaTime, Space.World);

        // Rotate towards the player
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void FireProjectile()
    {
        // Instantiate a homing projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        // Set the target for homing
        HomingProjectile homingProjectile = projectile.GetComponent<HomingProjectile>();
        if (homingProjectile != null)
        {
            homingProjectile.SetTarget(player);
        }
    }
}
