using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TruckBehavior : MonoBehaviour
{
    private Transform dumpYard;
    private GameManager gameManager;
    private GameObject currentTrashTarget;
    private GameObject carryingTrash;
    private Vector3 target;
    private float speed = 2f;
    private bool hasTrash = false;
    private float rotationSpeed = 5f; // Adjust for smooth turning

    public List<Transform> roadWaypoints; // Assign waypoints in Inspector
    private int currentWaypointIndex = 0;
    private bool goingToDumpYard = false;

    public void Initialize(Transform dumpYardRef, GameManager manager)
    {
        dumpYard = dumpYardRef;
        gameManager = manager;
        FindNextTrash();
    }

    void Update()
    {
        if (dumpYard == null || roadWaypoints.Count == 0) return;

        if (!hasTrash)
        {
            MoveToTrash();
        }
        else
        {
            MoveToDumpYard();
        }
    }

    void MoveToTrash()
    {
        if (currentTrashTarget == null)
        {
            FindNextTrash();
            if (currentTrashTarget == null) return;
        }

        target = currentTrashTarget.transform.position;
        RotateTowards(target);

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.5f)
        {
            // Pick up trash
            hasTrash = true;
            carryingTrash = currentTrashTarget;
            carryingTrash.transform.SetParent(transform);
            carryingTrash.transform.localPosition = new Vector3(0, 1, 0);
            currentTrashTarget = null;

            // Find the nearest road point to start following
            FindClosestRoadPoint();
            goingToDumpYard = true;
        }
    }

    void MoveToDumpYard()
    {
        if (currentWaypointIndex >= roadWaypoints.Count) return;

        Transform targetWaypoint = roadWaypoints[currentWaypointIndex];
        RotateTowards(targetWaypoint.position);
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // If truck reaches a waypoint, move to the next
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            currentWaypointIndex++;

            // 🛑 Last Waypoint Before Dump Yard: Release Trash Here
            if (currentWaypointIndex == roadWaypoints.Count - 1)
            {
                DropTrash();
            }
        }
    }

    void DropTrash()
    {
        if (carryingTrash != null)
        {
            carryingTrash.transform.SetParent(null);
            carryingTrash.transform.position = roadWaypoints[currentWaypointIndex].position + new Vector3(0, 0, Random.Range(-1, 1));
            carryingTrash = null;
            hasTrash = false;
            goingToDumpYard = false;

            // After dropping, find new trash
            FindNextTrash();
        }
    }

    void FindNextTrash()
    {
        currentTrashTarget = gameManager.GetNextTrash();
        if (currentTrashTarget != null)
        {
            target = currentTrashTarget.transform.position;
            currentWaypointIndex = 0;
            goingToDumpYard = false;
        }
    }

    void FindClosestRoadPoint()
    {
        float shortestDistance = Mathf.Infinity;
        int nearestIndex = 0;

        for (int i = 0; i < roadWaypoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, roadWaypoints[i].position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestIndex = i;
            }
        }

        currentWaypointIndex = nearestIndex;
    }

    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
}
