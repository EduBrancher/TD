using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] float timeBetweenWaves = 2f;
    [SerializeField] bool isLooping = false;
    [SerializeField] GameObject playerBase;
    WaveConfig currentWave;
    Pathfinder pathfinder;
    [SerializeField] bool testPathfinding;

    void Start()
    {   
        //Debug.Log(transform.position);
        //Debug.Log(playerBase.transform.position);
        pathfinder = GetComponentInParent<Pathfinder>();
 
        if (testPathfinding){
            List<Vector2> path = pathfinder.GetPath(playerBase.transform.position, transform.position);
            foreach (Vector2 step in path){
                Debug.Log(step.x + ", " + step.y);
            }
        }
        
        StartCoroutine(waveController());
    }

    public WaveConfig GetCurrentWave(){
        return currentWave;
    }

    IEnumerator waveController(){
        do {
            foreach (WaveConfig wave in waveConfigs){
            currentWave = wave;
            List<Vector2> path = pathfinder.GetPath(playerBase.transform.position, transform.position);
            for (int i = 0; i < currentWave.EnemyGroupsInWave(); i++){
                for (int j = 0; j < currentWave.GetEnemyCount(i); j++){
                    //Debug.Log(transform.position);
                    GameObject instance = Instantiate(currentWave.GetEnemyAtIndex(i), transform.position, Quaternion.identity, transform);
                    Enemy enemy = instance.GetComponent<Enemy>();
                    enemy.SetPath(path);
                    yield return new WaitForSeconds(currentWave.GetRandomTime());
                }
            }
            yield return new WaitForSeconds(timeBetweenWaves);
            }
        } while (isLooping);
        
    }

}
