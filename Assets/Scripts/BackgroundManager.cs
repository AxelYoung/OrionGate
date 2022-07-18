using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public Asteroids[] asteroids;
    int[] goalAmounts;
    List<List<GameObject>> asteroidsInScene = new List<List<GameObject>>();
    List<List<bool>> asteroidsInSceneVisible = new List<List<bool>>();
    List<List<float>> asteroidsInSceneTime = new List<List<float>>();

    public Collider2D viewport;

    public float driftingMultiplier;

    public float spawnHeightMin;
    public float spawnHeightMax;
    public float horizontalSpawnTolerance;

    void Start() {
        goalAmounts = new int[asteroids.Length];
        for (int i = 0; i < asteroids.Length; i++) {
            asteroidsInScene.Add(new List<GameObject>());
            asteroidsInSceneVisible.Add(new List<bool>());
            asteroidsInSceneTime.Add(new List<float>());
            goalAmounts[i] = Random.Range(asteroids[i].minAmount, asteroids[i].maxAmount);
            for (int j = 0; j < goalAmounts[i]; j++) {
                Sprite sprite = asteroids[i].sprites[Random.Range(0, asteroids[i].sprites.Length)];

                float xPosition = Random.Range(-viewport.bounds.extents.x - horizontalSpawnTolerance, viewport.bounds.extents.x + horizontalSpawnTolerance);
                float yPosition = Random.Range(-viewport.bounds.extents.y, viewport.bounds.extents.y);
                Vector3 position = new Vector3(xPosition, yPosition) + viewport.bounds.center;

                float speed = Random.Range(asteroids[i].minSpeed, asteroids[i].maxSpeed);
                asteroidsInScene[i].Add(NewAsteroid(sprite, asteroids[i].orderInLayer, position, new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed));
                asteroidsInSceneVisible[i].Add(false);
                asteroidsInSceneTime[i].Add(0f);
            }
        }
    }

    void Update() {
        for (int i = 0; i < asteroids.Length; i++) {
            if (asteroidsInScene[i].Count < goalAmounts[i]) {
                for (int j = 0; j < goalAmounts[i] - asteroidsInScene[i].Count; j++) {
                    Sprite sprite = asteroids[i].sprites[Random.Range(0, asteroids[i].sprites.Length)];

                    float boundsX = viewport.bounds.extents.x;
                    float boundsY = viewport.bounds.extents.y;

                    Vector2 position;
                    Vector2 direction;

                    float halfHeight = sprite.bounds.max.y;
                    float halfWidth = sprite.bounds.max.x;

                    if (Random.Range(0, 2) == 0) {
                        if (Random.Range(0, 2) == 0) {
                            position = new Vector2(Random.Range(-boundsX, boundsX), boundsY + halfHeight);
                            direction = Vector2.Lerp(new Vector2(-boundsX, boundsY), new Vector2(boundsX, boundsY), Random.Range(0f, 1f));
                        } else {
                            position = new Vector2(Random.Range(-boundsX, boundsX), -boundsY - halfHeight);
                            direction = Vector2.Lerp(new Vector2(-boundsX, -boundsY), new Vector2(boundsX, -boundsY), Random.Range(0f, 1f));
                        }
                    } else {
                        if (Random.Range(0, 2) == 0) {
                            position = new Vector2(boundsX + halfWidth, Random.Range(-boundsY, boundsY));
                            direction = Vector2.Lerp(new Vector2(boundsX, -boundsY), new Vector2(boundsX, boundsY), Random.Range(0f, 1f));
                        } else {
                            position = new Vector2(-boundsX - halfWidth, Random.Range(-boundsY, boundsY));
                            direction = Vector2.Lerp(new Vector2(-boundsX, -boundsY), new Vector2(-boundsX, boundsY), Random.Range(0f, 1f));
                        }
                    }
                    float speed = Random.Range(asteroids[i].minSpeed, asteroids[i].maxSpeed);
                    Vector2 velocity = -direction.normalized * speed;
                    asteroidsInScene[i].Add(NewAsteroid(sprite, asteroids[i].orderInLayer, position, velocity));
                    asteroidsInSceneVisible[i].Add(false);
                    asteroidsInSceneTime[i].Add(0f);
                }
            }
        }

        for (int i = 0; i < asteroidsInScene.Count; i++) {
            Queue<int> outOfBoundsIndexes = new Queue<int>();
            for (int j = 0; j < asteroidsInScene[i].Count; j++) {
                Vector2 position = asteroidsInScene[i][j].transform.position;
                float halfHeight = asteroidsInScene[i][j].GetComponent<SpriteRenderer>().sprite.bounds.max.y;
                float halfWidth = asteroidsInScene[i][j].GetComponent<SpriteRenderer>().sprite.bounds.max.x;
                asteroidsInSceneTime[i][j] += Time.deltaTime;
                if (position.y > -viewport.bounds.extents.y - halfHeight && position.y < viewport.bounds.extents.y + halfHeight && position.x > -viewport.bounds.extents.x - halfWidth && position.x < viewport.bounds.extents.x + halfWidth) {
                    asteroidsInSceneVisible[i][j] = true;
                } else {
                    if (asteroidsInSceneVisible[i][j] || asteroidsInSceneTime[i][j] > 5) {
                        outOfBoundsIndexes.Enqueue(j);
                    }
                }
            }

            if (outOfBoundsIndexes.Count > 0) {
                foreach (int outOfBoundsIndex in outOfBoundsIndexes) {
                    Destroy(asteroidsInScene[i][outOfBoundsIndex]);
                    asteroidsInScene[i].RemoveAt(outOfBoundsIndex);
                    asteroidsInSceneVisible[i].RemoveAt(outOfBoundsIndex);
                    asteroidsInSceneTime[i].RemoveAt(outOfBoundsIndex);
                }
                goalAmounts[i] = Random.Range(asteroids[i].minAmount, asteroids[i].maxAmount);
            }
        }
    }

    GameObject NewAsteroid(Sprite sprite, int orderInLayer, Vector2 position, Vector2 velocity) {
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

        Asteroid asteroidComponent = asteroid.AddComponent<Asteroid>();
        asteroidComponent.velocity = velocity;
        asteroidComponent.player = GameMaster.instance.player;

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