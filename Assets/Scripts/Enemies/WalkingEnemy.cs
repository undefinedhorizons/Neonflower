using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WalkingEnemy : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int _currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;
    private const float MaxDistance = 0.05f;
    [SerializeField] private float waitTime = 3f;
    private float _initialWaitTime;

    private void Start()
    {
        var vector3 = transform.position;
        vector3.y = Random.Range(waypoints[0].transform.position.y, waypoints[1].transform.position.y);
        transform.position = vector3;
        _currentWaypointIndex = Random.Range(0, 2);
        _initialWaitTime = Random.Range(waitTime, waitTime * 3);
        StartCoroutine(WalkingAI());
    }

    private IEnumerator WalkingAI()
    {
        yield return new WaitForSeconds(_initialWaitTime);

        while (enabled)
        {
            if (HasArrivedToWaypoint())
            {
                yield return new WaitForSeconds(waitTime);

                _currentWaypointIndex++;
                if (_currentWaypointIndex >= waypoints.Length)
                {
                    _currentWaypointIndex = 0;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    waypoints[_currentWaypointIndex].transform.position, Time.deltaTime * speed);
                yield return null;
            }
        }
    }


    private bool HasArrivedToWaypoint()
    {
        var distance = Vector2.Distance(waypoints[_currentWaypointIndex].transform.position, transform.position);
        return distance < MaxDistance;
    }
}