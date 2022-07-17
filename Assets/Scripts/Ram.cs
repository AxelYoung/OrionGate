using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Entity {

    public float speed;

    public float trackingSpeed;
    public float trackingDeadzone;

    Player player;
    Rigidbody2D rigidbody;

    public override void Start() {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (Vector2.Dot((player.transform.position - transform.position).normalized, Vector2.down) > 0.75f) {
            if (player.transform.position.x - transform.position.x < -trackingDeadzone) {
                rigidbody.velocity = (transform.up * (speed / 1.5f)) + -(transform.right * trackingSpeed);
            } else if (player.transform.position.x - transform.position.x > trackingDeadzone) {
                rigidbody.velocity = (transform.up * (speed / 1.5f)) + (transform.right * trackingSpeed);
            } else {
                rigidbody.velocity = transform.up * speed;
            }
        } else {
            rigidbody.velocity = transform.up * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == player.gameObject) {
            player.Hit(1);
            Hit(maxHealth);
        }
    }
}
