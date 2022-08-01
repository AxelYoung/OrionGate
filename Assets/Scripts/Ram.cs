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

    public override void Update() {
        base.Update();
        if (Vector2.Dot((player.transform.position - transform.position).normalized, transform.up) > 0.75f) {
            float angle = Vector2.SignedAngle(player.transform.position - transform.position, transform.up);
            if (angle > 5) {
                rigidbody.velocity = (transform.up * (speed / 1.5f)) + (transform.right * trackingSpeed);
            } else if (angle < -5) {
                rigidbody.velocity = (transform.up * (speed / 1.5f)) - (transform.right * trackingSpeed);
            } else {
                rigidbody.velocity = transform.up * speed;
            }
        } else {
            rigidbody.velocity = transform.up * speed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Entity entity = collision.gameObject.GetComponent<Entity>();
        if (entity != null) {
            entity.Hit(1);
            Hit(maxHealth);
        }
    }
}
