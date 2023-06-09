using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{   

    [SerializeField] float shakeMagnitude;
    [SerializeField] float shakeDuration;
    
    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {   
        initialPosition = transform.position;
    }

    public void Play(){
        StartCoroutine(Shake());
    }

    IEnumerator Shake(){
        float elapsedTime = 0;
        while(elapsedTime < shakeDuration){
            elapsedTime += Time.deltaTime;
            transform.position = transform.position + (Vector3) Random.insideUnitCircle * shakeMagnitude;
            yield return new WaitForEndOfFrame();
        }
        transform.position = initialPosition;
    }
}
