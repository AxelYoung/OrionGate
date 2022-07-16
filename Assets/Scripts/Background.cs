using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public GameObject asteroid;

    public float spawnRateMin;
    public float spawnRateMax;

    public float angleMin;
    public float angleMax;

    public float scaleMin;
    public float scaleMax;

    void Start() {
        for (int i = 0; i < 10; i++) {
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(angleMin, angleMax));
            GameObject currentEntity = Instantiate(asteroid, new Vector2(Random.Range(-5, 5), Random.Range(-transform.position.y, transform.position.y)), rotation);
            float scale = Random.Range(scaleMin, scaleMax);
            currentEntity.transform.localScale = new Vector3(scale, scale, 1);
        }
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(angleMin, angleMax));
        GameObject currentEntity = Instantiate(asteroid, new Vector2(Random.Range(-5, 5), transform.position.y), rotation);
        float scale = Random.Range(scaleMin, scaleMax);
        currentEntity.transform.localScale = new Vector3(scale, scale, 1);
        yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));
        StartCoroutine(SpawnLoop());
    }

}
