using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    // Target that the projectile tracks
    private Transform target;

    public float projectileSpeed;
    public int projectileDamage;

    public float MissileExplosionRadius;
    public int missileNumEnemiesHit;
    public float ExplosionDamagePercent;
    // Particle effects for the projectile's impact
    public GameObject impactEffect;

    // Figure out which target we're seeking (called in turret.cs)
    public void Seek(Transform _target) 
    {
        target = _target;
    }

    void Update()
    {
        // If we haven't found a target or the target reached the end, destroy the bullet
        if(target == null) 
        {
            Destroy(gameObject);
            return;
        }

        // Keep track of path the bullet needs to follow and the distance it will move in this frame
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = projectileSpeed * Time.deltaTime;

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

        // If we're using missiles, then damage all nearby enemies
        if(MissileExplosionRadius > 0f) 
            Explode();
        else // Otherwise damage target
            Damage(target, projectileDamage);

        Destroy(gameObject);
    }

    // Explosion when missile hits enemy
    void Explode() 
    {
        // Damage primary target with full damage
        Damage(target, projectileDamage);

        // Detect all objects nearby
        Collider[] colliders = Physics.OverlapSphere(transform.position, MissileExplosionRadius);

        // Find all nearby enemies and deal a percentage of damage to them
        SortedList<float, Collider> enemyList = new SortedList<float, Collider>(); // Keep track of every enemy and their distance away from the missile epicenter in a sorted fashion
        Collider primaryCollider = target.GetComponent<Collider>(); // Don't target the enemy that the missile hit
        foreach(Collider collider in colliders)
        {
            if(collider.tag == "Enemy" && collider != primaryCollider)
            {
                float distance = Vector3.Distance(target.position, collider.transform.position);
                enemyList.Add(distance, collider);
            }
        }

        int numEnemiesHit = 0;
        foreach(KeyValuePair<float, Collider> pair in enemyList)
        {
            if(numEnemiesHit == missileNumEnemiesHit)
                break;

            Damage(pair.Value.transform, ExplosionDamagePercent * projectileDamage);
            numEnemiesHit++;
        }
        enemyList.Clear();
    }

    // Damage an enemy when bullet hits it
    void Damage(Transform enemy, float damage)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if(e != null)
        {
            e.TakeDamage(damage);
        }
    }
    
}
