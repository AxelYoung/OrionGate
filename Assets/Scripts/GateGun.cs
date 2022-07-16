using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGun : MonoBehaviour {

    public Gate leftGate;
    public Gate rightGate;

    Vector2 leftGatePosition;
    Vector2 rightGatePosition;

    public bool canUse = false;

    void Update() {
        if (canUse) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                leftGatePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                leftGate.transform.position = leftGatePosition;
                if (!leftGate.gameObject.activeSelf) leftGate.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                rightGatePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                rightGate.transform.position = rightGatePosition;
                if (!rightGate.gameObject.activeSelf) rightGate.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                if (leftGate.gameObject.activeSelf) leftGate.gameObject.SetActive(false);
                if (rightGate.gameObject.activeSelf) rightGate.gameObject.SetActive(false);
            }
        }
    }
}
