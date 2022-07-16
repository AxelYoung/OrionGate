using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity {

    public float speed;
    public float lifespan;

    float lifetime;

    SpriteRenderer renderer;
    TrailRenderer trailRenderer;

    public override void Start() {
        base.Start();
        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
        renderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    void Update() {
        renderer.color = GameMaster.instance.activeTheme;
        trailRenderer.startColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        trailRenderer.endColor = Color.HSVToRGB(GameMaster.instance.activeThemeHSV.r, GameMaster.instance.activeThemeHSV.g, 0.9f);
        lifetime += Time.deltaTime;
        if (lifetime >= lifespan) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Entity enemy = collision.gameObject.GetComponent<Entity>();
        if (enemy != null) {
            enemy.Hit(1);
            Hit(1);
        }
    }
}
