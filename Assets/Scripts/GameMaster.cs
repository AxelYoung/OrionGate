using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public Player player;
    public GateGun gateGun;
    public WaveSpawner waveSpawner;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartGame() {
        player.GetComponent<Animator>().enabled = false;
        player.canMove = true;
        waveSpawner.StartSpawnLoop();
        gateGun.canUse = true;
    }

    public void RestartGame() {
        Invoke("Reload", 3);
    }

    void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
