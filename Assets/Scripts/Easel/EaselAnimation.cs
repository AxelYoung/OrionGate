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
    float frameTime;

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
            frameTime += Time.deltaTime;
            if (frameTime >= currentSegment.frameLength) {
                frameTime = 0;
                currentSpriteIndex++;
                if (currentSpriteIndex < currentSegment.sprites.Length) {
                    SwapSprite(currentSprite);
                } else {
                    currentSegmentIndex++;
                    if (currentSegmentIndex < currentClip.segments.Length) {
                        SwapSprite(currentSprite);
                    } else {
                        playing = false;
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
        frameTime = 0;
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