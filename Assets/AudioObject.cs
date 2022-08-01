using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour {

    bool active = false;

    // Update is called once per frame
    void Update() {
        if (GetComponent<AudioSource>().isPlaying == false && active) {
            Destroy(gameObject);
        }
    }

    public void Play(AudioClip clip) {
        GetComponent<AudioSource>().PlayOneShot(clip);
        active = true;
    }

}
