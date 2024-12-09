using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointSystem : MonoBehaviour
{
    [SerializeField] Image waypoint;
    [SerializeField] Transform waypointLocation;
    [SerializeField] Camera cam;
    void Update()
    {
        waypoint.transform.position = cam.WorldToScreenPoint(waypointLocation.position);
    }
}
