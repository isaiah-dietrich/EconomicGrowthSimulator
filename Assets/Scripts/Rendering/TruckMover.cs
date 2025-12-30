using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckMover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 10f;
    
    // We store the path points here
    private Queue<Vector3> _pathPoints = new Queue<Vector3>();
    private Vector3 _currentDestination;
    private bool _isMoving = false;

    public void SetPath(List<Vector3> pathPoints)
    {
        _pathPoints.Clear();
        
        // Enqueue all points so we can process them one by one
        foreach (Vector3 point in pathPoints)
        {
            _pathPoints.Enqueue(point);
        }

        // Start moving to the first point
        if (_pathPoints.Count > 0)
        {
            _currentDestination = _pathPoints.Dequeue();
            _isMoving = true;
        }
    }

    void Update()
    {
        if (!_isMoving) return;

        // 1. Move towards the destination
        // MoveTowards ensures we don't overshoot the target
        transform.position = Vector3.MoveTowards(transform.position, _currentDestination, speed * Time.deltaTime);

        // 2. Rotate to face the destination
        Vector3 direction = (_currentDestination - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }

        // 3. Check if we reached the waypoint
        if (Vector3.Distance(transform.position, _currentDestination) < 0.05f)
        {
            // If there are more points, get the next one. Otherwise, stop.
            if (_pathPoints.Count > 0)
            {
                _currentDestination = _pathPoints.Dequeue();
            }
            else
            {
                _isMoving = false;
                Debug.Log("Truck reached the destination!");
            }
        }
    }
}