using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {

    public float speed;
    public float lifespan;

    float lifetime;

    public override void Start() {
        base.Start();
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
    }

    void Update() {
        lifetime += Time.deltaTime;
        if (lifetime >= lifespan) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Entity enemy = collision.gameObject.GetComponent<Entity>();
        if (enemy != null) {
            enemy.Hit(1);
            Hit(1);
        }
    }
}
