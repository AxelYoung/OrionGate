using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance { get; private set; }

    public Player player;
    public GateGun gateGun;
    public WaveManager waveSpawner;

    public int volume;

    public int activeThemeIndex = 0;
    public Color[] themes;

    public Color activeTheme { get { return themes[activeThemeIndex]; } }

    public RawImage renderTexture;

    float rainbowTime;
    public float rainbowSpeedMultipler;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    void Update() {
        SetRainbow();
        renderTexture.color = activeTheme;
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    public void StartGame() {
        player.GetComponent<Animator>().enabled = false;
        player.canMove = true;
        waveSpawner.StartWaves();
        gateGun.canUse = true;
    }

    public void RestartGame() {
        Invoke("Reload", 1.5f);
    }

    void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetRainbow() {
        rainbowTime += Time.deltaTime * rainbowSpeedMultipler;
        themes[themes.Length - 1] = Color.HSVToRGB(rainbowTime, 1, 1);
        if (rainbowTime >= 1) {
            rainbowTime = 0;
        }
    }
}
