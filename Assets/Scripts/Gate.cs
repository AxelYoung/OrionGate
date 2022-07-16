using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    public Gate alternateGate;

    List<GameObject> objectsEntered = new List<GameObject>();

    SpriteRenderer renderer;
    SpriteRenderer particleRenderer;

    void Start() {
        renderer = GetComponent<SpriteRenderer>();
        particleRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update() {
        renderer.color = GameMaster.instance.activeTheme;
        particleRenderer.color = GameMaster.instance.activeTheme;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (alternateGate.gameObject.activeSelf) {
            ITeleportable teleportObj = collision.GetComponent<ITeleportable>();
            if (teleportObj != null) {
                if (teleportObj.Teleportable) {
                    Vector3 offset = new Vector2(transform.position.x - collision.transform.position.x, 0);
                    collision.transform.position = alternateGate.transform.position - offset;
                    objectsEntered.Add(collision.gameObject);
                    teleportObj.Teleportable = false;
                }

            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (alternateGate.gameObject.activeSelf) {
            if (!objectsEntered.Contains(collision.gameObject)) {
                ITeleportable teleportObj = collision.GetComponent<ITeleportable>();
                if (teleportObj != null) {
                    teleportObj.Teleportable = true;
                }
            } else {
                objectsEntered.Remove(collision.gameObject);
            }
        }
    }
}
