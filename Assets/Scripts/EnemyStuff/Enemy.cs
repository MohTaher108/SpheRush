using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    // Enemy starting health (for ratioing the health bar)
    public float startHealth = 100;
    private float currentHealth;

    // Starting speed of our enemy (startSpeed is used to manage slows)
    public float startSpeed = 10f;
    [HideInInspector] // keep public but hidden for enemyMovement script
    public float speed;

    // Money the user gains when the enemy dies
    public int worth = 50;
    public int LivesCount = 1;

    // Check to see if it's a fake enemy, which doesn't count as a life
    public bool isFake = false;

    [Header("Slow stuff")]
    // Slow lingering effect
    public float slowTimerStart = 3f;
    [HideInInspector]
    public float slowTimer;

    // Unity stuff that user shouldn't mess with
    [Header("Unity Stuff")]
    public Image healthBar;
    public GameObject deathEffect;
    public ParticleSystem slowEffect;

    // This exists to prevent a bug where an enemy dies multiple times due to the Destroy() method taking too long
    private bool isDead = false;

    void Start()
    {
        speed = startSpeed;
        currentHealth = startHealth;
    }

    // Slow lingering effect
    void Update()
    {
        slowTimer -= Time.deltaTime;

        if(slowTimer <= 0)
        {
            speed = startSpeed;
            slowEffect.Stop();
        }
    }

    // Deal damage to enemy (return whether enemy has died or not)
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / startHealth;

        // If enemy runs out of hp, kill them
        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // Enemy died
    void Die()
    {
        // Set that the enemy died so this function isn't called again on the same enemy
        isDead = true;

        // Give player money for killing enemy
        PlayerStats.Money += worth;

        // Instantiate enemy death particle effects then destroy them after 5 seconds
        GameObject effect = (GameObject) Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        // Report an enemy death
        WaveSpawner.enemyDied();

        // Destroy enemy
        Destroy(gameObject);
    }
    
    // Slow the enemy by pct% (pct is a number from 0 to 1)
    public void Slow(float pct) 
    {
        speed = startSpeed * (1 - pct);
        // Add a lingering effect
        slowTimer = slowTimerStart;
        slowEffect.Play();
    }
}
