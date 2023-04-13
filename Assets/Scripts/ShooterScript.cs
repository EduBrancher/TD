using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{      

    [Header("General")]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 5f;
    [SerializeField] float firingRate = 0.5f;
    [SerializeField] GameObject projectile;

    
    Coroutine firingCoroutine;
    GameObject firedProjectile;
    [HideInInspector]
    public bool recentlyFired = false;
    [HideInInspector]
    public bool isFiring;
    float firingTimer = 0f;
    AudioPlayer audioPlayer;
    Turret parentTurret;
    GameObject target;

    private void Awake() {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        parentTurret = GetComponentInParent<Turret>();
        target = parentTurret.GetTarget();
    }
    

    IEnumerator FireContinuously(){
        recentlyFired = true;
        while (true){
            if (target != null){
                firedProjectile = Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, parentTurret.GetRotation()));
                firedProjectile.GetComponent<MonoBehaviour>().enabled = true;
                firedProjectile.GetComponent<DamageDealer>().SetTarget(target, projectileSpeed);
                Destroy(firedProjectile, projectileLifeTime);
            }
            float firingTime = firingRate;
            yield return new WaitForSeconds(firingTime);
        }
    }

    private void Update() {
        target = parentTurret.GetTarget();
        if (target != null && firingCoroutine == null && !recentlyFired){
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (target == null && firingCoroutine != null){
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
            
        }
        /*firingTimer += Time.deltaTime;
        if (firingTimer > firingRate){
            recentlyFired = false;
            firingTimer = 0;
        }*/
    }
}
