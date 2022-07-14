using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    public float minSpeed;
    public float maxSpeed;

    void Start() {
        float trueSpeed = Random.Range(minSpeed, maxSpeed);
        GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-trueSpeed / 3f, trueSpeed / 3f), -trueSpeed);
    }
}
