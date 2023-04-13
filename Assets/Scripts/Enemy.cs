using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{   

    [SerializeField] float moveSpeed = 1f;
    List<Vector2> waypoints;
    int waypointIndex;

    void Update()
    {
        FollowPath();
    }

    public void SetPath(List<Vector2> path){
        waypoints = path;
        waypointIndex = 0;
    }

    void FollowPath(){
        if (waypointIndex < waypoints.Count){
            //Debug.Log(waypointIndex);
            //Debug.Log(waypoints.Count);
            Vector3 targetPos = waypoints[waypointIndex];
            //Debug.Log(targetPos);
            //Debug.Log(transform.position);
            float delta = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, delta);
            if (transform.position == targetPos){
                waypointIndex++;
            }
        }
        else{
            Destroy(gameObject);
        }
    }
}
