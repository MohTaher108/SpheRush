using UnityEngine;

public class Turret : MonoBehaviour
{

    // Target we're aiming at, and a reference to the enemy object
    private Transform target;
    private Enemy targetEnemy;

    // Range that we can shoot
    [Header("General")]
    public float range = 15f;


    // Prefab of the turret's bullets, how often the turret shoots, and when the turret shoots
    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    // Using Laser boolean, dps, and slow percent
    [Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowAmount = .5f; // slow enemy by slowAmount% (so enemy speed is 1 - slowAmount)

    // LineRenderer, particleSystem, and lightEffect
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;


    // Part to rotate (so we don't rotate the base of the turret), and turn speed of our turret while tracking the enemy (smaller = smoother)
    [Header("Unity Setup Fields")]
    public Transform partToRotate;
    public float turnSpeed = 10f;
    // Tag for enemies so the turret can reference all enemies, and the transform of the bullet's starting point
    public string enemyTag = "Enemy";
    public Transform firePoint;


    // Start is called before the first frame update
    void Start()
    {
        // Repeat the function UpdateTarget every 0.5 seconds starting from 0 seconds
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // Update which target the turret is looking at
    void UpdateTarget()
    {
        // reference all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        // keep track of the closest enemy's distance and transform
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // Iterate through all the enemies to find the closest one to the turret
        foreach (GameObject enemy in enemies)
        {
            // Distance between turret and enemy
            float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // If an enemy exists and is in range, KILL EM
        if(nearestEnemy != null && shortestDistance <= range) 
        {
            target = nearestEnemy.transform;
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        // Otherwise set target to null so Update() doesn't mess up 
        else
        {
            target = null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // If UpdateTarget() didn't find an enemy, stop any lasers and give up
        if(target == null) 
        {
            // If the turret is an active laser beamer, then disable the lineRenderer, particle system, and light effect
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

        // Fire at the target (either using a laser or bullet)
        if(useLaser)
            FireLaser();
        else
            FireBullet();
    }

    // Make the turret face the enemy
    void LockOnTarget()
    {
        // Find the distance of the enemy target, and keep track of the turret's rotation needed to face the enemy
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);

        // Turn the Quaternion rotation into eulerAngles (x, y, z angles) since it's easier to manage
        // Also, Quaternion.Lerp() is for making the movements smooth instead of snappy
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        // Turn the turret to only the y angle we found
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // Handle everything to do with shooting the laser
    void FireLaser()
    {
        // Deal damage to and slow enemy
        targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        // Enable lineRenderer, particle system, and light effect if needed
        if(!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        // Set the lineRenderer to start at the laser beamer and end at the enemy
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        // Set the particle system's rotation to face the turret
        Vector3 dir = firePoint.position - target.position;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
        // Set the particle system's position to be at the edge of the enemy
        impactEffect.transform.position = target.position + dir.normalized;
    }

    void FireBullet()
    {
        // When the timer counts down, shoot and reset fireCountDown
        if(fireCountdown <= 0f)
        {
            SpawnBullet();
            fireCountdown = 1f / fireRate;
        }

        // Decrement fireCountDown
        fireCountdown -= Time.deltaTime;    
    }

    // Shoot a bullet
    void SpawnBullet()
    {
        // Make the bullet and extract the bullet component of it
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        // If a bullet was formed, set its target to the current target
        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }

    // Draw the range the turret has
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
