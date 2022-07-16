using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameMaster gameMaster;

    Animator animator { get { return GetComponent<Animator>(); } }

    public Animator[] startEvents;

    bool played = false;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !played) {
            animator.SetTrigger("Start");
            Invoke("PlayStartEvents", 1.85f);
            played = true;
        }
    }

    void PlayStartEvents() {
        foreach (Animator eventAnimator in startEvents) {
            eventAnimator.SetTrigger("Start");
        }
        Invoke("StartGame", 2);
    }

    void StartGame() { gameMaster.StartGame(); }
}
