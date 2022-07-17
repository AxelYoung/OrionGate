using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    public float decayTime;
    public float totalTime;

    SpriteRenderer renderer;

    Color[] colors = new Color[4];

    int currentColor;

    void Start() {
        decayTime = Random.Range(0.1f, 0.3f);

        renderer = GetComponent<SpriteRenderer>();

        colors[0] = Color.HSVToRGB(0, 0, 0.9f);
        colors[1] = Color.HSVToRGB(0, 0, 0.65f);
        colors[2] = Color.HSVToRGB(0, 0, 0.32f);
        colors[3] = Color.HSVToRGB(0, 0, 0.1f);

        for (int i = 0; i < 4; i++) {
            if (withinColorMargin(colors[i])) {
                currentColor = i;
                return;
            }
        }
    }

    void Update() {
        totalTime += Time.deltaTime;
        if (totalTime >= decayTime) {
            currentColor++;
            renderer.color = colors[currentColor];
            decayTime = Random.Range(0.1f, 0.3f);
            totalTime = 0;
        }
        if (currentColor == 3) {
            Destroy(gameObject);
        }
    }

    bool withinColorMargin(Color comparison) {
        float inputH, inputS, inputV, comparisonH, comparisonS, comparisonV;
        Color.RGBToHSV(renderer.color, out inputH, out inputS, out inputV);
        Color.RGBToHSV(comparison, out comparisonH, out comparisonS, out comparisonV);
        if (inputV > comparisonV - 0.01f && inputV < comparisonV + 0.01f) {
            return true;
        } else {
            return false;
        }
    }
}
