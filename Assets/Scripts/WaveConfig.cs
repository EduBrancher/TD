using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "WaveConfig", fileName = "New Asset Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] List<GameObject> enemyTypes;
    [SerializeField] List<int> enemyCounts;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float varianceBetweenSpawns = 0.5f;
    [SerializeField] float minTimeBetweenSpawns = 0.25f;

    public int EnemyGroupsInWave(){
        return enemyTypes.Count;
    }

    public int GetEnemyCount(int index){
        return enemyCounts[index];
    }

    public GameObject GetEnemyAtIndex(int index){
        return enemyTypes[index];
    }

    public float GetRandomTime(){
        float randomTime = Random.Range(timeBetweenSpawns - varianceBetweenSpawns, timeBetweenSpawns + varianceBetweenSpawns);
        //Debug.Log(Mathf.Clamp(randomTime, minTimeBetweenSpawns, timeBetweenSpawns + varianceBetweenSpawns));
        return Mathf.Clamp(randomTime, minTimeBetweenSpawns, timeBetweenSpawns + varianceBetweenSpawns);
    }

}
