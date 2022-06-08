using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    // Starting speed and current speed of our enemy (startSpeed is used to manage slows)
    public float startSpeed = 10f;
    [HideInInspector] // hides the speed from our inspector, but it's still public
    public float speed;

    // Enemy current health
    public float health = 100;

    // Money the user gains when the enemy dies
    public int worth = 50;

    // Particle effects when enemy dies
    public GameObject deathEffect;

    void Start()
    {
        speed = startSpeed;
    }

    // Deal damage to enemy
    public void TakeDamage(float amount)
    {
        health -= amount;

        // If enemy runs out of hp, kill them
        if(health <= 0)
        {
            Die();
        }
    }

    // Slow the enemy by pct% (pct is a number from 0 to 1)
    public void Slow(float pct) 
    {
        speed = startSpeed * (1 - pct);
    }

    // Enemy died
    void Die()
    {
        // Give player money for killing enemy
        PlayerStats.Money += worth;

        // Instantiate enemy death particle effects then destroy them after 5 seconds
        GameObject effect = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        // Destroy enemy
        Destroy(gameObject);
    }
}
