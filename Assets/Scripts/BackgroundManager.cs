using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public Asteroids[] asteroids;
    int[] goalAmounts;
    List<List<GameObject>> asteroidsInScene = new List<List<GameObject>>();

    public Collider2D viewport;

    public float driftingMultiplier;

    public float spawnHeightMin;
    public float spawnHeightMax;
    public float horizontalSpawnTolerance;

    void Start() {
        goalAmounts = new int[asteroids.Length];
        for (int i = 0; i < asteroids.Length; i++) {
            asteroidsInScene.Add(new List<GameObject>());
            goalAmounts[i] = Random.Range(asteroids[i].minAmount, asteroids[i].maxAmount);
            for (int j = 0; j < goalAmounts[i]; j++) {
                Sprite sprite = asteroids[i].sprites[Random.Range(0, asteroids[i].sprites.Length)];

                float xPosition = Random.Range(-viewport.bounds.extents.x - horizontalSpawnTolerance, viewport.bounds.extents.x + horizontalSpawnTolerance);
                float yPosition = Random.Range(-viewport.bounds.extents.y, viewport.bounds.extents.y);
                Vector3 position = new Vector3(xPosition, yPosition) + viewport.bounds.center;

                float speed = Random.Range(asteroids[i].minSpeed, asteroids[i].maxSpeed);
                asteroidsInScene[i].Add(NewAsteroid(sprite, asteroids[i].orderInLayer, position, speed));
            }
        }
    }

    void Update() {
        for (int i = 0; i < asteroids.Length; i++) {
            if (asteroidsInScene[i].Count < goalAmounts[i]) {
                for (int j = 0; j < goalAmounts[i] - asteroidsInScene[i].Count; j++) {
                    Sprite sprite = asteroids[i].sprites[Random.Range(0, asteroids[i].sprites.Length)];

                    float xPosition = Random.Range(-viewport.bounds.extents.x - horizontalSpawnTolerance, viewport.bounds.extents.x + horizontalSpawnTolerance);
                    float yPosition = viewport.bounds.extents.y + sprite.bounds.max.y + Random.Range(spawnHeightMin, spawnHeightMax);
                    Vector3 position = new Vector3(xPosition, yPosition) + viewport.bounds.center;

                    float speed = Random.Range(asteroids[i].minSpeed, asteroids[i].maxSpeed);
                    asteroidsInScene[i].Add(NewAsteroid(sprite, asteroids[i].orderInLayer, position, speed));
                }
            }
        }

        for (int i = 0; i < asteroidsInScene.Count; i++) {
            Queue<int> outOfBoundsIndexes = new Queue<int>();
            for (int j = 0; j < asteroidsInScene[i].Count; j++) {
                Vector2 position = asteroidsInScene[i][j].transform.position;
                float halfHeight = asteroidsInScene[i][j].GetComponent<SpriteRenderer>().sprite.bounds.max.y;
                if (position.y < -viewport.bounds.extents.y - halfHeight) {
                    outOfBoundsIndexes.Enqueue(j);
                }
            }

            if (outOfBoundsIndexes.Count > 0) {
                foreach (int outOfBoundsIndex in outOfBoundsIndexes) {
                    Destroy(asteroidsInScene[i][outOfBoundsIndex]);
                    asteroidsInScene[i].RemoveAt(outOfBoundsIndex);
                }
                goalAmounts[i] = Random.Range(asteroids[i].minAmount, asteroids[i].maxAmount);
            }
        }
    }

    GameObject NewAsteroid(Sprite sprite, int orderInLayer, Vector2 position, float speed) {
        GameObject asteroid = new GameObject("Asteroid");
        asteroid.transform.position = position;
        asteroid.transform.parent = transform;
        asteroid.layer = 8;

        SpriteRenderer renderer = asteroid.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = orderInLayer;
        renderer.flipX = Random.Range(0, 2) == 0 ? true : false;
        renderer.flipY = Random.Range(0, 2) == 0 ? true : false;

        Rigidbody2D rigidbody = asteroid.AddComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        rigidbody.velocity = new Vector2(Random.Range(-speed * driftingMultiplier, speed * driftingMultiplier), -speed);

        return asteroid;
    }

}

[System.Serializable]
public struct Asteroids {
    public Sprite[] sprites;
    public float minSpeed;
    public float maxSpeed;
    public int minAmount;
    public int maxAmount;
    public int orderInLayer;
}