using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public float minSpeed;
    public float maxSpeed;

    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        float trueSpeed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-trueSpeed / 3f, trueSpeed / 3f), -trueSpeed);
    }

    void Update() {
        spriteRenderer.color = GameMaster.instance.activeTheme;
    }
}
