using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject entity;

    public float spawnRateMin;
    public float spawnRateMax;

    public float angleMin;
    public float angleMax;

    public float scaleMin;
    public float scaleMax;

    void Start() {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(angleMin, angleMax));
        GameObject currentEntity = Instantiate(entity, new Vector2(Random.Range(-5, 5), transform.position.y), rotation);
        float scale = Random.Range(scaleMin, scaleMax);
        currentEntity.transform.localScale = new Vector3(scale, scale, 1);
        yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));
        StartCoroutine(SpawnLoop());
    }
}
