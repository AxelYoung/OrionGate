using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multishot : Entity {

    public float shotFrequency;

    public GameObject bulletPrefab;

    public Transform[] barrels;

    public float moveSpeed;
    public float rotationSpeed;

    public Vector2 dir;

    public override void Start() {
        base.Start();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
        transform.localRotation = Quaternion.Euler(0, 0, transform.localEulerAngles.z + (rotationSpeed * Time.deltaTime)); ;
        transform.position += (Vector3)dir * Time.deltaTime * moveSpeed;
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(shotFrequency);
        for (int i = 0; i < barrels.Length; i++) {
            Instantiate(bulletPrefab, barrels[i].transform.position + (barrels[i].transform.up * 0.3f), barrels[i].transform.rotation);
        }
        StartCoroutine(Shoot());
    }
}
