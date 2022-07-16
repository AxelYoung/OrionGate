using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, ITeleportable {

    public int maxHealth;
    public int currentHealth;

    public GameObject destroyParticles;

    float hitAnimationLength = 0.1f;

    SpriteRenderer renderer;

    public virtual void Start() {
        currentHealth = maxHealth;
        renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Hit(int damageAmount) {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) {
            Destroy(gameObject);
            GameObject particles = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            particles.GetComponent<ParticleSystem>().startColor = GameMaster.instance.activeTheme;
        } else {
            StartCoroutine(HitAnimation());
        }
    }

    IEnumerator HitAnimation() {
        Sprite currentSprite = renderer.sprite;
        renderer.sprite = hitSprite(currentSprite);
        yield return new WaitForSeconds(hitAnimationLength);
        renderer.sprite = currentSprite;
    }

    Sprite hitSprite(Sprite currentSprite) {
        Texture2D currentTexture = currentSprite.texture;
        Texture2D newTexture = new Texture2D(currentTexture.width, currentTexture.height, TextureFormat.RGBA32, 0, false);
        newTexture.filterMode = FilterMode.Point;
        for (int x = 0; x < currentTexture.width; x++) {
            for (int y = 0; y < currentTexture.height; y++) {
                if (currentTexture.GetPixel(x, y).a != 0) {
                    newTexture.SetPixel(x, y, Color.HSVToRGB(0, 0, 0.9f));
                } else {
                    newTexture.SetPixel(x, y, Color.clear);
                }
            }
        }
        newTexture.Apply();
        Sprite sprite = Sprite.Create(newTexture, new Rect(0.0f, 0.0f, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f), 8);
        return sprite;
    }

    bool teleportable = true;
    public bool Teleportable {
        get {
            return teleportable;
        }
        set {
            teleportable = value;
        }
    }
}
