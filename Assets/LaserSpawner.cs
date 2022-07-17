using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSpawner : MonoBehaviour {

    public GameObject laserPrefab;

    public Collider2D viewport;

    Sprite sprite;

    float boundsX, boundsY;
    float halfHeight;

    GameObject laserInstance;
    bool instantiated = false;

    float spawnTime = 0.5f;
    float totalTime;
    Vector2 startPos;
    Vector2 goalPos;

    void Start() {
        sprite = laserPrefab.GetComponent<SpriteRenderer>().sprite;
        boundsX = viewport.bounds.extents.x;
        boundsY = viewport.bounds.extents.y;
        halfHeight = sprite.bounds.extents.y;
        Invoke("Spawn", 4);
    }

    void Update() {
        if (instantiated) {
            totalTime += Time.deltaTime;
            laserInstance.transform.position = Vector2.Lerp(startPos, goalPos, totalTime / spawnTime);
            if (totalTime > spawnTime) {
                laserInstance.transform.position = goalPos;
                laserInstance.GetComponent<Animator>().SetTrigger("Charge");
                instantiated = false;
            }
        }
    }

    void Spawn() {
        Vector2 position;
        Quaternion rotation;
        if (Random.Range(0, 2) == 0) {
            if (Random.Range(0, 2) == 0) {
                position = new Vector2(Random.Range(-boundsX, boundsX), boundsY + halfHeight);
                goalPos = new Vector2(position.x, position.y - (halfHeight * 2));
                rotation = Quaternion.Euler(0, 0, 180);
            } else {
                position = new Vector2(Random.Range(-boundsX, boundsX), -boundsY - halfHeight);
                goalPos = new Vector2(position.x, position.y + (halfHeight * 2));
                rotation = Quaternion.Euler(0, 0, 0);
            }
        } else {
            if (Random.Range(0, 2) == 0) {
                position = new Vector2(boundsX + halfHeight, Random.Range(-boundsY, boundsY));
                goalPos = new Vector2(position.x - (halfHeight * 2), position.y);
                rotation = Quaternion.Euler(0, 0, 90);
            } else {
                position = new Vector2(-boundsX - halfHeight, Random.Range(-boundsY, boundsY));
                goalPos = new Vector2(position.x + (halfHeight * 2), position.y);
                rotation = Quaternion.Euler(0, 0, 270);
            }
        }
        laserInstance = Instantiate(laserPrefab, position, rotation);
        startPos = position;
        instantiated = true;
    }
}
