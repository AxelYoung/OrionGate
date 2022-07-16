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

    SpriteRenderer renderer;
    TrailRenderer trailRendererL;
    TrailRenderer trailRendererR;

    public override void Start() {
        base.Start();
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        trailRendererL = transform.GetChild(0).GetComponent<TrailRenderer>();
        trailRendererR = transform.GetChild(1).GetComponent<TrailRenderer>();
    }

    void Update() {
        hpBar.color = GameMaster.instance.activeTheme;
        renderer.color = GameMaster.instance.activeTheme;
        trailRendererL.startColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        trailRendererR.startColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        trailRendererL.endColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        trailRendererR.endColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
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
                Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
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
