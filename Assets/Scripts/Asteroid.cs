using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public Vector2 velocity;
    public Player player;

    Rigidbody2D rigidbody;

    void Start() => rigidbody = GetComponent<Rigidbody2D>();

    // Update is called once per frame
    void Update() {
        rigidbody.velocity = velocity / 5f;
        rigidbody.velocity -= ((player.moveDirection * player.rigidbody.velocity.magnitude) / 10f) * (velocity.magnitude);
        rigidbody.velocity = new Vector2((player.clampedX ? velocity.x / 5f : rigidbody.velocity.x), (player.clampedY ? velocity.y / 5f : rigidbody.velocity.y));
    }
}
