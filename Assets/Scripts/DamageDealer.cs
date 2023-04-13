using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;
    GameObject target;
    float projectileSpeed = 5f;
    bool hasTarget = false;

    void Awake(){
        Debug.Log(target);
    }

    private void Start() {
        
    }

    private void Update(){
        if (hasTarget && target != null){
            this.transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, projectileSpeed*Time.deltaTime);
        }
        else{
            Destroy(gameObject);
        }
    }

    public int GetDamage(){
        return damage;
    }

    public void Hit(){
        Destroy(gameObject);
    }

    public GameObject GetTarget(){
        return target;
    }

    public void SetTarget(GameObject target, float projectileSpeed){
        this.target = target;
        this.hasTarget = true;
        this.projectileSpeed = projectileSpeed;
    }

  
}