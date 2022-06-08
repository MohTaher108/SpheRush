using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    // target that the bullet tracks
    private Transform target;

    // Speed of the bullet
    public float speed = 70f;

    // Damage count of bullet
    public int damage = 50;

    // Explosion radius of missile and particle effects for the bullet
    public float explosionRadius = 0f;
    public GameObject impactEffect;

    // Figure out which target we're seeking (called in turret.cs)
    public void Seek(Transform _target) 
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        // If we haven't found a target or the target reached the end, destroy the bullet
        if(target == null) {
            Destroy(gameObject);
            return;
        }

        // Keep track of path the bullet needs to follow and the distance it will move in this frame
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // If we reached the bullet, call HitTarget()
        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // If we haven't reached the bullet, move the bullet forward
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        // Rotate the projectile to look at the target (mostly for missiles)
        transform.LookAt(target);
    }

    // When we hit the target, instantiate the particle effects and destroy the bullet
    void HitTarget()
    {
        GameObject effectIns = (GameObject) Instantiate(impactEffect, transform.position, transform.rotation);
        // Destroy particle effects after 5 seconds to clean up hierarchy
        Destroy(effectIns, 5f);

        if(explosionRadius > 0f) {
            Explode();
        } else {
            Damage(target);
        }

        Destroy(gameObject);
    }

    // Explosion when missile hits enemy
    void Explode() 
    {
        // Detect all objects nearby
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        // Filter out anything besides the enemies, and then damage the enemies
        foreach(Collider collider in colliders)
            if(collider.tag == "Enemy")
                Damage(collider.transform);
    }

    // Damage an enemy when bullet hits it
    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if(e != null)
        {
            e.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
