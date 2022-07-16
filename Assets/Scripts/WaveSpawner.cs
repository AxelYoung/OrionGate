using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public GameObject entity;

    public float spawnRateMin;
    public float spawnRateMax;

    public void StartSpawnLoop() {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        Instantiate(entity, new Vector2(Random.Range(-5, 5), transform.position.y), Quaternion.Euler(0, 0, 180));
        yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));
        StartCoroutine(SpawnLoop());
    }
}
