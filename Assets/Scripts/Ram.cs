using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ram : Entity {

    public float speed;

    public float trackingSpeed;
    public float trackingDeadzone;

    Player player;
    Rigidbody2D rigidbody;
    SpriteRenderer renderer;
    TrailRenderer trailRenderer;

    public override void Start() {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        renderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update() {
        renderer.color = GameMaster.instance.activeTheme;
        trailRenderer.startColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        trailRenderer.endColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        if (Vector2.Dot((player.transform.position - transform.position).normalized, Vector2.down) > 0.75f) {
            if (player.transform.position.x - transform.position.x < -trackingDeadzone) {
                rigidbody.velocity = new Vector2(-trackingSpeed, -speed / 1.5f);
            } else if (player.transform.position.x - transform.position.x > trackingDeadzone) {
                rigidbody.velocity = new Vector2(trackingSpeed, -speed / 1.5f);
            } else {
                rigidbody.velocity = new Vector2(0, -speed);
            }
        } else {
            rigidbody.velocity = new Vector2(0, -speed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == player.gameObject) {
            //Instantiate(bulletHitParticle, transform.position, Quaternion.identity);
            player.Hit(1);
            Hit(maxHealth);
        }
    }
}
