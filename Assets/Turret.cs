using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity {

    public float shotFrequency;

    public GameObject bulletPrefab;

    Transform barrel;

    Player player;

    public override void Start() {
        base.Start();
        player = GameMaster.instance.player;
        barrel = transform.GetChild(0);
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update() {
        barrel.up = player.transform.position - barrel.position;
    }

    IEnumerator Shoot() {
        yield return new WaitForSeconds(shotFrequency);
        Instantiate(bulletPrefab, barrel.transform.position, barrel.transform.rotation);
        StartCoroutine(Shoot());
    }
}
