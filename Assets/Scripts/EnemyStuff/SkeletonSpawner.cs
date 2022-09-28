using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class SkeletonSpawner : MonoBehaviour
{
    public int skeletonsSpawned;
    public int skeletonSpawnRate;

    public float startSpawnTimer;
    private float spawnTimer;

    private EnemyMovement parentEnemyMovementScript;
    public GameObject Enemy_Skeleton;

    void Start()
    {
        spawnTimer = 0f;
        parentEnemyMovementScript = gameObject.GetComponent<EnemyMovement>();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if(spawnTimer <= 0f)
        {
            spawnTimer = startSpawnTimer;
            StartCoroutine(spawnSkeletons());
        }
    }

    IEnumerator spawnSkeletons()
    { 
        int skeletonsRemaining = skeletonsSpawned;
        while(skeletonsRemaining > 0)
        {
            GameObject curSkeleton = Instantiate(Enemy_Skeleton, transform.position, transform.rotation);
            curSkeleton.GetComponent<EnemyMovement>().changeWaypoint(parentEnemyMovementScript.waypointIndex);
            GameStats.EnemiesAlive++;
            skeletonsRemaining--;
            yield return new WaitForSeconds(1f / skeletonSpawnRate);
        }
    }
}
