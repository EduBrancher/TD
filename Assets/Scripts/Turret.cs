using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{   

    List<GameObject> targets = new List<GameObject>();
    GameObject currentTarget = null;

    float rotationToTarget;

    private void Awake() {
        this.rotationToTarget = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy"){
            targets.Add(other.gameObject);
            if (currentTarget == null){
                UpdateTarget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        targets.Remove(other.gameObject);
        if (other.gameObject == currentTarget){
            currentTarget = null;
            UpdateTarget();
        }
    }

    private void UpdateTarget(){
        if (targets.Count > 0){
            float closestDistance = Mathf.Infinity;
            foreach (GameObject enemy in targets){
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance){
                    closestDistance = distance;
                    currentTarget = enemy;
                }
            }
        }
        else{
            currentTarget = null;
        }
    }

    private void Update() {
        if (currentTarget == null || currentTarget.activeInHierarchy == false){
            UpdateTarget();
        }
        if (currentTarget != null){
            rotationToTarget = FindRotationToTarget();
        }
    }

    private float FindRotationToTarget(){
        float dx = currentTarget.transform.position.x - transform.position.x;
        float dy = currentTarget.transform.position.y - transform.position.y;
        float tangent = Mathf.Abs(dy)/Mathf.Abs(dx);
        float arc = Mathf.Atan(tangent);
        float angleInDegrees = arc * 180/Mathf.PI;
        if (dx > 0 && dy > 0){
            return -(90-angleInDegrees);
        }
        else if (dx > 0 && dy < 0){
            return -(90+angleInDegrees);
        }
        else if (dx < 0 && dy < 0){
            return -(270-angleInDegrees);
        }
        else{
            return -(270 + angleInDegrees);
        }


    }

    public float GetRotation(){
        return rotationToTarget;
    }

    public GameObject GetTarget(){
        return currentTarget;
    }
}
