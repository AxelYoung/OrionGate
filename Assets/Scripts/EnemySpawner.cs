using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float spawnRate;

    public GameObject enemy;

    void Start() {
        StartCoroutine(EnemySpawnLoop());
    }

    IEnumerator EnemySpawnLoop() {
        Quaternion rotation = Quaternion.Euler(0, 0, 180);
        Instantiate(enemy, new Vector2(Random.Range(-5, 5), transform.position.y), rotation);
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(EnemySpawnLoop());
    }
}
