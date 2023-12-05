using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float lifetime = 2f; // Time before projectile disappears
    private Transform target;

    void Start()
    {
        // Destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (target == null)
        {
            // No target, destroy the projectile
            Destroy(gameObject);
            return;
        }

        // Move towards the target in the X and Y axes only
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.z = 0f; // Ignore the Z-axis
        transform.Translate(directionToTarget.normalized * speed * Time.deltaTime, Space.World);

        // Rotate towards the target
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
            Destroy(gameObject);
        }
    }
}
