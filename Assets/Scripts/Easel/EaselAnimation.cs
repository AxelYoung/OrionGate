using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EaselAnimation : MonoBehaviour {
    public EaselClip[] clips;
    public bool playOnStart;

    bool playing;
    EaselClip currentClip;
    Segment currentSegment { get { return currentClip.segments[currentSegmentIndex]; } }
    int currentSegmentIndex;
    Sprite currentSprite { get { return currentSegment.sprites[currentSpriteIndex]; } }
    int currentSpriteIndex;
    float currentFrameLength {
        get {
            return Ease.linear(currentSpriteIndex + 1, currentSegment.sprites.Length, 1);
        }
    }

    float totalFrameTime;

    int renderer;
    SpriteRenderer spriteRenderer;
    Image image;


    void Start() {
        GetRenderer();
        if (playOnStart) {
            PlayClip(0);
        }
    }

    void Update() {
        if (playing) {
            totalFrameTime += Time.deltaTime;
            if (totalFrameTime >= currentFrameLength) {
                currentSpriteIndex++;
                if (currentSpriteIndex < currentSegment.sprites.Length) {
                    SwapSprite(currentSprite);
                } else {
                    currentSegmentIndex++;
                    totalFrameTime = 0;
                    if (currentSegmentIndex < currentClip.segments.Length) {
                        currentSpriteIndex = 0;
                        SwapSprite(currentSprite);
                    } else {
                        if (currentClip.loop) {
                            PlayClip(0);
                        } else {
                            playing = false;
                        }
                    }
                }
            }
        }
    }

    public void PlayClip(int index) {
        playing = true;
        currentClip = clips[index];
        currentSegmentIndex = 0;
        currentSpriteIndex = 0;
        totalFrameTime = 0;
        SwapSprite(currentSprite);
    }

    void GetRenderer() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            renderer = 0;
        } else {
            image = GetComponent<Image>();
            if (image != null) {
                renderer = 1;
            } else {
                Debug.LogError("No renderer found");
            }
        }
    }

    void SwapSprite(Sprite sprite) {
        switch (renderer) {
            case 0:
                spriteRenderer.sprite = sprite;
                break;
            case 1:
                image.sprite = sprite;
                break;
        }
    }
}

public static class Ease {

    public static float linear(float input, float destination, float rateOfChange) {
        return rateOfChange * input / destination;
    }

    public static float inOutCubic(float input, float destination, float rateOfChange) {
        input /= destination / 2;
        if (input < 1) return rateOfChange / 2 * input * input * input;
        input -= 2;
        return rateOfChange / 2 * (input * input * input + 2);
    }
}