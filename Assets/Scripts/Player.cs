using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {

    public float moveSpeed;

    [System.NonSerialized] public Vector2 moveDirection;
    Vector2 targetVelocity;
    [System.NonSerialized] public Rigidbody2D rigidbody;

    public GameObject bulletPrefab;
    public float fireRate;
    public Transform bulletSpawn;
    public Transform bulletSpawnR;

    float bulletTimer;

    public Image hpBar;

    public bool canMove = false;

    public bool clampedX;
    float clampXNeg = -5.75f;
    float clampXPos = 5.65f;
    public bool clampedY;
    float clampY = 8.2f;

    bool left = false;

    public AudioClip shootSound;

    AudioSource audioSource;

    public override void Start() {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Update() {
        base.Update();
        if (canMove) {
            Movement();
            Weapons();
        }
    }

    void Movement() {
        moveDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        moveDirection = Vector2.ClampMagnitude(moveDirection, 1);
        targetVelocity = moveDirection * moveSpeed;

        Vector3 unclampedPos = transform.position;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, clampXNeg, clampXPos), Mathf.Clamp(transform.position.y, -clampY, clampY));
        clampedX = transform.position.x == clampXNeg || transform.position.x == clampXPos;
        clampedY = Mathf.Abs(transform.position.y) == clampY;
    }

    void FixedUpdate() {
        rigidbody.velocity += targetVelocity;
    }

    void Weapons() {
        if (Input.GetKey(KeyCode.Space)) {
            bulletTimer += Time.deltaTime;
            if (bulletTimer >= fireRate) {
                Instantiate(bulletPrefab, left ? bulletSpawn.position : bulletSpawnR.position, transform.rotation);
                audioSource.PlayOneShot(shootSound);
                left = !left;
                bulletTimer = 0f;
            }
        } else {
            bulletTimer = 0f;
        }
    }

    public override void Hit(int damageAmount) {
        base.Hit(damageAmount);
        hpBar.transform.localScale = new Vector3((float)currentHealth / maxHealth, 1, 1);
        if (currentHealth <= 0) {
            GameMaster.instance.RestartGame();
        }
    }
}
