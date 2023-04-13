using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{   
    
    [Header("ShootingClip")]
    [SerializeField] [Range(0f, 1f)] float shootingVolume;
    [SerializeField] AudioClip shootingClip;
    [Header("DamageClip")]
    [SerializeField] [Range(0f, 1f)] float damageVolume;
    [SerializeField] AudioClip damageClip;

    
    static AudioPlayer instance;
    //public singleton (global acessible)
    /*public AudioPlayer getInstance(){
        return instance;
    }*/

    private void Awake() {
        ManageSingleton();
    }
    //private singleton (non global acessible)
    private void ManageSingleton(){
        if (instance != null){
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);  
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayShootingClip(){
        AudioSource.PlayClipAtPoint(shootingClip, Camera.main.transform.position, shootingVolume);
    }

    public void PlayDamageClip(){
        AudioSource.PlayClipAtPoint(damageClip, Camera.main.transform.position, damageVolume);
    }
}
