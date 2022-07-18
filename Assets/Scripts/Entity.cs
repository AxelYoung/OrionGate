using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, ITeleportable {

    public int maxHealth;
    public int currentHealth;

    float hitAnimationLength = 0.1f;

    SpriteRenderer renderer;

    bool dead = false;

    public virtual void Start() {
        currentHealth = maxHealth;
        renderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Hit(int damageAmount) {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) {
            if (!dead) {
                Destroy(gameObject);
                Explode();
                dead = true;
            }
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
        print(currentSprite.pivot);
        print(sprite.pivot);
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

    void Explode() {
        GameObject explosion = new GameObject("Explosion");
        explosion.transform.position = transform.position;
        Texture2D texture = renderer.sprite.texture;
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                Color pixel = texture.GetPixel(x, y);
                if (pixel.a != 0) {
                    GenerateChunk(x, y, pixel, explosion.transform);
                }
            }
        }
        explosion.transform.rotation = transform.rotation;
    }

    void GenerateChunk(float x, float y, Color color, Transform parent) {
        GameObject chunk = new GameObject();
        chunk.transform.parent = parent;
        chunk.transform.localPosition = new Vector3(x / 8f, y / 8f);
        chunk.layer = 8;

        Texture2D chunkTexture = new Texture2D(1, 1);
        chunkTexture.SetPixel(0, 0, Color.white);
        chunkTexture.filterMode = FilterMode.Point;
        chunkTexture.Apply();

        SpriteRenderer chunkRenderer = chunk.AddComponent<SpriteRenderer>();
        chunkRenderer.sprite = Sprite.Create(chunkTexture, new Rect(0.0f, 0.0f, chunkTexture.width, chunkTexture.height), new Vector2(0.5f, 0.5f), 8);
        chunkRenderer.color = color;

        Rigidbody2D rigidbody = chunk.AddComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        rigidbody.isKinematic = true;

        chunk.AddComponent<Chunk>();
    }
}
