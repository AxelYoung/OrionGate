using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, ITeleportable {

    public int maxHealth;
    public int currentHealth;

    public GameObject destroyParticles;

    public Material hitMaterial;
    Material defaultMaterial;

    float hitAnimationLength = 0.1f;

    SpriteRenderer renderer;

    public virtual void Start() {
        currentHealth = maxHealth;
        renderer = GetComponent<SpriteRenderer>();
        defaultMaterial = renderer.material;
    }

    public virtual void Hit(int damageAmount) {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) {
            Destroy(gameObject);
            Instantiate(destroyParticles, transform.position, Quaternion.identity);
        } else {
            StartCoroutine(HitAnimation());
        }
    }

    IEnumerator HitAnimation() {
        renderer.material = hitMaterial;
        yield return new WaitForSeconds(hitAnimationLength);
        renderer.material = defaultMaterial;
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
