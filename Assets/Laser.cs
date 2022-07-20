using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    SpriteRenderer renderer;
    BoxCollider2D collider;

    Transform cannon;

    public LayerMask layerMask;

    public GameObject laserPrefab;

    GameObject laserInstance;

    public bool main = false;

    void Start() {
        if (main) {
            cannon = transform.parent;
            renderer = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update() {
        if (main) {
            collider.size = new Vector2(collider.size.x, renderer.size.y);
            RaycastHit2D hit = Physics2D.Raycast(cannon.position, cannon.up, 100, layerMask);
            if (hit.transform != null) {
                Gate gate = hit.transform.GetComponent<Gate>();
                if (gate.alternateGate.gameObject.activeSelf) {
                    cannon.GetComponent<Animator>().enabled = false;
                    renderer.size = new Vector2(renderer.size.x, Vector2.Distance(cannon.position, hit.transform.position));
                    transform.localPosition = new Vector2(0, renderer.size.y / 2f);
                    if (laserInstance == null) {
                        Vector2 position = gate.alternateGate.transform.position + (gate.alternateGate.transform.up * 20);
                        laserInstance = Instantiate(laserPrefab, position, gate.alternateGate.transform.rotation);
                    } else {
                        Vector2 position = gate.alternateGate.transform.position + (gate.alternateGate.transform.up * 20);
                        laserInstance.transform.position = position;
                        laserInstance.transform.rotation = gate.alternateGate.transform.rotation;
                    }
                } else {
                    renderer.size = new Vector2(renderer.size.x, 100);
                    transform.localPosition = new Vector2(0, (renderer.size.y / 2f) + 0.3f);
                    if (laserInstance != null) { Destroy(laserInstance); }
                }
            } else {
                renderer.size = new Vector2(renderer.size.x, 100);
                transform.localPosition = new Vector2(0, (renderer.size.y / 2f) + 0.3f);
                if (laserInstance != null) { Destroy(laserInstance); }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null) {
            entity.Hit(1);
        }
    }

    float hitDelay = 0.2f;
    float totalHitTime = 0f;

    void OnTriggerStay2D(Collider2D other) {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null) {
            totalHitTime += Time.deltaTime;
            if (totalHitTime >= hitDelay) {
                entity.Hit(1);
                totalHitTime = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null) {
            totalHitTime = 0;
        }
    }
}
