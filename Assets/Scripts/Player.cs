using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

    public float moveSpeed;

    Vector2 moveDirection;
    Vector2 targetVelocity;
    Rigidbody2D rigidbody;

    public GameObject bulletPrefab;
    public float fireRate;
    public Transform bulletSpawn;

    float bulletTimer;

    public Image hpBar;
    public Sprite[] hpBarSprites;

    public bool canMove = false;

    public override void Start() {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (canMove) {
            Movement();
            Weapons();
        }
    }

    void Movement() {
        moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDirection = Vector2.ClampMagnitude(moveDirection, 1);
        targetVelocity = moveDirection * moveSpeed;

        rigidbody.velocity = targetVelocity;

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -5.75f, 5.65f), Mathf.Clamp(transform.position.y, -8.2f, 8.2f));
    }

    void Weapons() {
        if (Input.GetKey(KeyCode.Space)) {
            bulletTimer += Time.deltaTime;
            if (bulletTimer >= fireRate) {
                Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
                bulletTimer = 0f;
            }
        } else {
            bulletTimer = 0f;
        }
    }

    public override void Hit(int damageAmount) {
        base.Hit(damageAmount);
        hpBar.sprite = hpBarSprites[currentHealth];
        if (currentHealth <= 0) {
            GameMaster.instance.RestartGame();
        }
    }
}
