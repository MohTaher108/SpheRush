using UnityEngine;

public class ConstantFiringTurret : MonoBehaviour
{

    // Target we're aiming at
    public Transform target;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool useLaser = false;

    // Laser effects
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;

    [Header("Unity Setup Fields")]
    // Turrets rotating body
    public Transform partToRotate;
    // Turn speed when tracking enemy (smaller = smoother)
    public float turnSpeed = 10f;
    // Projectiles' spawn location
    public Transform firePoint;

    void Start()
    {
        if(useLaser)
            FireLaser();
    }

    void Update()
    {
        if(!useLaser)
            FireBullet();
        else
            this.enabled = false;
    }

    void FireLaser()
    {
        // Enable Laser effects
        lineRenderer.enabled = true;
        impactEffect.Play();
        impactLight.enabled = true;

        // Set the lineRenderer to start at the laser beamer's firepoint and end at the enemy
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
        // When the timer counts down, shoot
        if(fireCountdown <= 0f)
        {
            SpawnBullet();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;    
    }

    void SpawnBullet()
    {
        // Instantiate the bullet and set its target
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if(bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
