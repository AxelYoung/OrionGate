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

    void Update() {
        if (canUse) {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                leftGatePosition = MouseToWorldSpace();
                leftGate.transform.position = leftGatePosition;
                if (!leftGate.gameObject.activeSelf) leftGate.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1)) {
                rightGatePosition = MouseToWorldSpace();
                rightGate.transform.position = rightGatePosition;
                if (!rightGate.gameObject.activeSelf) rightGate.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                if (leftGate.gameObject.activeSelf) leftGate.gameObject.SetActive(false);
                if (rightGate.gameObject.activeSelf) rightGate.gameObject.SetActive(false);
            }
        }
    }

    Vector2 MouseToWorldSpace() {
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(renderTexture.rectTransform, Input.mousePosition, null, out point);
        point /= 8;
        return point;
    }
}
