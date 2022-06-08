using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    // Keep track of the next waypoint
    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemy;

    // initialize target to first waypoint
    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
    }

    // Go from waypoint to waypoint
    void Update() {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

        // We reached the waypoint (not == 0 cuz the enemy won't perfectly reach the waypoint)
        if(Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

        enemy.speed = enemy.startSpeed;
    }

    // Get the next waypoint in order
    void GetNextWaypoint() {
        // Reached the end
        if(waypointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    // Destroy the enemy when they reach the end
    void EndPath()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
