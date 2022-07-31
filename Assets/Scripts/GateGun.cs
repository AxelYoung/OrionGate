using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GateGun : MonoBehaviour {

    public Gate leftGate;
    public Gate rightGate;

    Vector2 leftGatePosition;
    Vector2 rightGatePosition;

    public bool canUse = false;

    public RawImage renderTexture;

    public Camera displayCamera;
    public Camera renderCamera;

    public GameObject cursor;

    void Start() => Cursor.visible = false;

    void Update() {
        cursor.transform.position = MouseToWorldSpace();
        if (canUse) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                leftGatePosition = MouseToWorldSpace(true);
                leftGate.transform.position = leftGatePosition;
                if (!leftGate.gameObject.activeSelf) leftGate.gameObject.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Mouse0)) {
                Vector2 mousePos = MouseToWorldSpace();
                if (Vector2.Distance(mousePos, leftGatePosition) > 1f) {
                    float angle = Mathf.Atan2(leftGatePosition.y - mousePos.y, leftGatePosition.x - mousePos.x) * 180 / Mathf.PI;
                    if (angle > -45 && angle < 45) {
                        leftGate.transform.rotation = Quaternion.Euler(0, 0, 90);
                    } else if (angle > -135 && angle < -45) {
                        leftGate.transform.rotation = Quaternion.Euler(0, 0, 0);
                    } else if (angle > 45 && angle < 135) {
                        leftGate.transform.rotation = Quaternion.Euler(0, 0, 180);
                    } else {
                        leftGate.transform.rotation = Quaternion.Euler(0, 0, -90);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                rightGatePosition = MouseToWorldSpace(true);
                rightGate.transform.position = rightGatePosition;
                if (!rightGate.gameObject.activeSelf) rightGate.gameObject.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Mouse1)) {
                Vector2 mousePos = MouseToWorldSpace();
                if (Vector2.Distance(mousePos, rightGatePosition) > 1f) {
                    float angle = Mathf.Atan2(rightGatePosition.y - mousePos.y, rightGatePosition.x - mousePos.x) * 180 / Mathf.PI;
                    if (angle > -45 && angle < 45) {
                        rightGate.transform.rotation = Quaternion.Euler(0, 0, 90);
                    } else if (angle > -135 && angle < -45) {
                        rightGate.transform.rotation = Quaternion.Euler(0, 0, 0);
                    } else if (angle > 45 && angle < 135) {
                        rightGate.transform.rotation = Quaternion.Euler(0, 0, 180);
                    } else {
                        rightGate.transform.rotation = Quaternion.Euler(0, 0, -90);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                if (leftGate.gameObject.activeSelf) leftGate.gameObject.SetActive(false);
                if (rightGate.gameObject.activeSelf) rightGate.gameObject.SetActive(false);
            }
        }
    }

    Vector2 MouseToWorldSpace(bool bounded = false) {
        Vector2 point = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(renderTexture.rectTransform, Input.mousePosition, null, out point);
        point /= 8;
        if (bounded) point = new Vector2(point.x > 5.6f ? 5.6f : point.x < -5.6f ? -5.6f : point.x, point.y > 8.1f ? 8.1f : point.y < -8.1f ? -8.1f : point.y);
        return point;
    }
}
