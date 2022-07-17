using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {

    public float speed;
    public float lifespan;

    float lifetime;

    Rigidbody2D rigidbody;

    public override void Start() {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        lifetime += Time.deltaTime;
        if (lifetime >= lifespan) {
            Destroy(gameObject);
        }
        rigidbody.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Entity enemy = collision.gameObject.GetComponent<Entity>();
        if (enemy != null) {
            enemy.Hit(1);
            Hit(1);
        }
    }
}
