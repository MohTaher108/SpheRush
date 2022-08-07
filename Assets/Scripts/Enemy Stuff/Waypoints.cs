using UnityEngine;

public class Waypoints : MonoBehaviour
{
    
    public static Transform[] waypoints;

    void Awake()
    {
        // Copy over all the transforms of every waypoint
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

}
