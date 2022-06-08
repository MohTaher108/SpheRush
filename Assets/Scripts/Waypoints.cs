using UnityEngine;

public class Waypoints : MonoBehaviour
{
    
    // Keep track of the transform of every waypoint
    public static Transform[] points;

    void Awake()
    {
        // Copy over all the transforms of every waypoint
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }

}
