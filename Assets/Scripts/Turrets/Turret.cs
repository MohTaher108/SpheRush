using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour
{

    // Target we're aiming at, and a reference to the enemy object
    private Transform targetEnemyTransform;
    private Enemy targetEnemyScript;

    [Header("General")]
    public float turretRange = 15f;

    [Header("Use Bullets/Missiles (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;
    // Create a linear ramping effect for the laser turret's laser's damage and width
    public float maxDPS = 30f;
    private float currentDPS;
    public float maxTime = 2f;
    private float currentLaserTime;
    private float lineWidth; // Keep track of the lineRenderer's starting width to create a beam that gets bigger as DPS increases
    // Slow enemy to (1 - slowAmount)
    public float slowAmount = .5f;

    // Laser effects
    public ParticleSystem impactEffect;
    public Light impactLight;
    public LineRenderer lineRenderer;

    [Header("Unity Setup Fields")]
    // Turrets rotating body
    public Transform partToRotate;
    // Turn speed when tracking enemy (smaller = smoother)
    public float turnSpeed = 10f;
    // Tag for enemies so the turret can reference all enemies
    public string enemyTag = "Enemy";
    // Projectiles' spawn location
    public Transform firePoint;

    void Start()
    {
        if(useLaser)
            lineWidth = lineRenderer.startWidth;
        // Repeat the function UpdateTarget every 0.5 seconds starting from 0 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update which target the turret is looking at
    void UpdateTarget()
    {
        // Reference all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // Keep track of the closest enemy's distance and transform
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Iterate through all the enemies to find the closest one to the turret
        foreach (GameObject enemy in enemies)
        {
            // Distance between turret and enemy
            float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance) 
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // If enemies nearby, target closest enemy
        if(nearestEnemy != null && shortestDistance <= turretRange) 
        {
            targetEnemyTransform = nearestEnemy.transform;
            Enemy previousEnemyScript = targetEnemyScript;
            targetEnemyScript = nearestEnemy.GetComponent<Enemy>();
            // Reset laser
            if(useLaser && previousEnemyScript != targetEnemyScript)
            {
                currentDPS = 0;
                currentLaserTime = 0;
                lineRenderer.startWidth = 0;
            }
        } else // Otherwise set target to null so Update() can stop any active lasers
        {
            targetEnemyTransform = null;
        }

    }

    void Update()
    {
        // Decrement bullet timer (I put it here so it counts down even when there's no targets)
        if(!useLaser)
            fireCountdown -= Time.deltaTime;

        // If UpdateTarget() didn't find an enemy, stop any lasers and give up
        if(targetEnemyTransform == null) 
        {
            // If the turret has an active laser beamer, then disable the laser effects
            if(useLaser && lineRenderer.enabled) 
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }

            return;
        }

        // Aim at the target
        LockOnTarget();

        if(useLaser)
            FireLaser();
        else
            FireBullet();
    }

    // Make the turret face the enemy
    void LockOnTarget()
    {
        // Compute the distance and rotation of the enemy target
        Vector3 dir = targetEnemyTransform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        // Use Lerp() to make the turret rotate smoothly instead of snapping, and turn the Quaternion into euler angles since it's easier to manage
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        // Turn the turret to only the y angle we found
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void FireLaser()
    {
        // Update laser's time
        if(currentLaserTime < maxTime)
        {
            // Increment the current laser's time and make sure it doesn't go over the maxTime
            currentLaserTime += Time.deltaTime;
            currentLaserTime = Mathf.Clamp(currentLaserTime, 0f, maxTime);

            // Update the DPS and lineRenderer's width (the lineRenderer's width goes between 0.15f and 0.3f)
            currentDPS = (currentLaserTime / maxTime) * maxDPS;
            lineRenderer.startWidth = (currentLaserTime / maxTime) * lineWidth;
        }

        targetEnemyScript.TakeDamage(currentDPS * Time.deltaTime);
        targetEnemyScript.Slow(slowAmount);

        // Enable laser effects, if needed
        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        // Set the lineRenderer to start at the laser beamer's firepoint and end at the enemy
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, targetEnemyTransform.position);

        // Set the laser's effect's rotation to face the turret
        Vector3 dir = firePoint.position - targetEnemyTransform.position;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        // Set the laser's effect's position to be at the edge of the enemy
        impactEffect.transform.position = targetEnemyTransform.position + dir.normalized;

        // Also, fix the enemy's lingering slow effect's transform's rotation and position
        targetEnemyScript.slowEffect.transform.rotation = Quaternion.LookRotation(dir);
        targetEnemyScript.slowEffect.transform.position = targetEnemyTransform.position + dir.normalized;
    }

    // When the timer counts down, shoot
    void FireBullet()
    {
        if(fireCountdown > 0f)
            return;

        // Instantiate the bullet and set its target
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null)
        {
            bullet.Seek(targetEnemyTransform);
        }        
        
        fireCountdown = 1f / fireRate;
    }

    // Draw the range of the turret (unused function)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }
}
