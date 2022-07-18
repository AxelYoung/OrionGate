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
        rigidbody.velocity -= ((player.moveDirection * player.rigidbody.velocity.magnitude / player.moveSpeed)) * (velocity.magnitude);
    }
}
