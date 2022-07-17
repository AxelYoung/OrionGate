using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public Asteroids[] asteroids;
    int[] goalAmounts;
    List<List<GameObject>> asteroidsInScene = new List<List<GameObject>>();

    public Collider2D viewport;

    void Start() {
        goalAmounts = new int[asteroids.Length];
        for (int i = 0; i < asteroids.Length; i++) {
            Asteroids currentType = asteroids[i];
            asteroidsInScene.Add(new List<GameObject>());
            goalAmounts[i] = Random.Range(currentType.minAmount, currentType.maxAmount);
            for (int j = 0; j < goalAmounts[i]; j++) {
                Sprite sprite = currentType.sprites[Random.Range(0, currentType.sprites.Length)];
                float xPosition = Random.Range(-viewport.bounds.extents.x, viewport.bounds.extents.x);
                float yPosition = Random.Range(-viewport.bounds.extents.y, viewport.bounds.extents.y);
                Vector3 position = new Vector3(xPosition, yPosition) + viewport.bounds.center;
                float speed = Random.Range(currentType.minSpeed, currentType.maxSpeed);
                asteroidsInScene[i].Add(NewAsteroid(sprite, currentType.orderInLayer, position, speed));
            }
        }
    }

    GameObject NewAsteroid(Sprite sprite, int orderInLayer, Vector2 position, float speed) {
        GameObject asteroid = new GameObject("Asteroid");
        asteroid.transform.position = position;
        asteroid.transform.parent = transform;
        SpriteRenderer renderer = asteroid.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = orderInLayer;
        renderer.flipX = Random.Range(0, 2) == 0 ? true : false;
        renderer.flipY = Random.Range(0, 2) == 0 ? true : false;
        Rigidbody2D rigidbody = asteroid.AddComponent<Rigidbody2D>();
        rigidbody.isKinematic = true;
        rigidbody.velocity = new Vector2(Random.Range(-speed / 10f, speed / 10f), -speed);
        BoxCollider2D collider = asteroid.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
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