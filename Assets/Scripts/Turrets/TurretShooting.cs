using UnityEngine;
using Unity.RemoteConfig;
using System.Collections;

public class TurretShooting : MonoBehaviour
{

    [Header("General")]
    public float turretRange;

    [Header("Use Bullets/Missiles (default)")]
    public GameObject bulletPrefab;
    public float fireRate;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;
    // Create a linear ramping effect for the laser turret's laser's damage and width
    public float PercentDamage;
    public float maxDPS;
    private float currentDPS;
    public float maxTime;
    private float currentLaserTime;
    private float lineWidth; // Keep track of the lineRenderer's starting width to create a beam that gets bigger as DPS increases
    // Slow enemy to (1 - slowAmount)
    public float slowAmount;

    // Laser effects
    public ParticleSystem impactEffect;
    public Light impactLight;
    public LineRenderer lineRenderer;

    [Header("Unity Setup Fields")]
    // Turrets rotating body
    public Transform partToRotate;
    // Projectiles' spawn location
    public Transform firePoint;

    // Target we're aiming at, and a reference to the enemy object
    private Transform enemyTargetTransform;
    private Enemy enemyTargetScript;
    // Tag for enemies so the turret can reference all enemies
    private string enemyTag = "Enemy";
    // Turn speed when tracking enemy (smaller = smoother)
    private float turnSpeed = 10f;

    // // Remote Config stuff
    // public struct userAttributes { }
    // public struct appAttributes { }

    // // Fetch remote configs
    // void Awake()
    // {
    //     if(useLaser)
    //     {
    //         ConfigManager.FetchCompleted += SetTurretMaxDPS;
    //         ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    //     }
    // }

    // // Change the MaxDPS of our turret
    // void SetTurretMaxDPS(ConfigResponse response)
    // {
    //     if(isUpgraded)
    //         maxDPS = ConfigManager.appConfig.GetFloat("LaserTurretUpgradedMaxDPS");
    //     else
    //         maxDPS = ConfigManager.appConfig.GetFloat("LaserTurretMaxDPS");
    // }

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
            enemyTargetTransform = nearestEnemy.transform;
            Enemy previousEnemyScript = enemyTargetScript;
            enemyTargetScript = nearestEnemy.GetComponent<Enemy>();
            // Reset laser
            if(useLaser && previousEnemyScript != enemyTargetScript)
            {
                currentDPS = 0;
                currentLaserTime = 0;
                lineRenderer.startWidth = 0;
            }
        } else // Otherwise set target to null so Update() can stop any active lasers
        {
            enemyTargetTransform = null;
        }

    }

    void Update()
    {
        // Decrement bullet timer (I put it here so it counts down even when there's no targets)
        if(!useLaser)
            fireCountdown -= Time.deltaTime;

        // If UpdateTarget() didn't find an enemy, stop any lasers and give up
        if(enemyTargetTransform == null) 
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
        Vector3 dir = enemyTargetTransform.position - transform.position;
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
            currentDPS = (currentLaserTime / maxTime) * (PercentDamage * enemyTargetScript.startHealth);
            currentDPS = Mathf.Clamp(currentDPS, 0f, maxDPS);
            lineRenderer.startWidth = (currentLaserTime / maxTime) * lineWidth;
        }

        enemyTargetScript.TakeDamage(currentDPS * Time.deltaTime);
        enemyTargetScript.Slow(slowAmount);

        // Enable laser effects, if needed
        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        // Set the lineRenderer to start at the laser beamer's firepoint and end at the enemy
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, enemyTargetTransform.position);

        // Set the laser's effect's rotation to face the turret
        Vector3 dir = firePoint.position - enemyTargetTransform.position;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        // Set the laser's effect's position to be at the edge of the enemy
        impactEffect.transform.position = enemyTargetTransform.position + dir.normalized;

        // Also, fix the enemy's lingering slow effect's transform's rotation and position
        enemyTargetScript.slowEffect.transform.rotation = Quaternion.LookRotation(dir);
        enemyTargetScript.slowEffect.transform.position = enemyTargetTransform.position + dir.normalized;
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
            bullet.Seek(enemyTargetTransform);
        }        
        
        fireCountdown = 1f / fireRate;
    }

    // Draw the range of the turret (unused function)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }

    // // Remove the updating of the turret DPS when it's destroyed, so the ConfigManager doesn't call a function on an inexistant object
    // void OnDestroy()
    // {
    //     ConfigManager.FetchCompleted -= SetTurretMaxDPS;
    // }
}
