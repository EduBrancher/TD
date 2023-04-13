using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 100;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] bool shakeCameraOnHit = false;
    [SerializeField] float scoreGrantedOnDeath = 100f;
    [SerializeField] bool isPlayerHealth;
    CameraShake cameraShake;
    AudioPlayer audioPlayer;
    //Scoring score;
    //UIController UI;
    int baseHealth;
    
    private void Awake() {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        //score = FindObjectOfType<Scoring>();
        //UI = FindObjectOfType<UIController>();
        baseHealth = health;
    }


    public int GetHealth(){
        return health;
    }

    public void TakeDamage(int damage){
        health = health - damage;
        if (isPlayerHealth){
            //UI.setHealthSlider((float)health/baseHealth);
            Debug.Log((float)health/baseHealth);
        }
        PlayHitEffect();
        //audioPlayer.PlayDamageClip();
        if (cameraShake != null && shakeCameraOnHit){
            cameraShake.Play();
        }
        if (health <= 0){
            Destroy(gameObject);
            //score.incrementScore(scoreGrantedOnDeath);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        DamageDealer collisionDamage = other.GetComponent<DamageDealer>();

        if (collisionDamage != null){
            if (collisionDamage.GetTarget() != this.gameObject){
                return;
            }
            TakeDamage(collisionDamage.GetDamage());
            collisionDamage.Hit();
        }
    }

    public void PlayHitEffect(){
        if (hitEffect != null){
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    public int getHealth(){
        return health;
    }
}
