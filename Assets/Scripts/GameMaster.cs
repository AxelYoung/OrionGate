using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance { get; private set; }

    public Player player;
    public GateGun gateGun;
    public WaveSpawner waveSpawner;

    public int volume;

    public int activeThemeIndex = 0;
    public Color[] themes;

    public Color activeTheme { get { return themes[activeThemeIndex]; } }
    public Color activeThemeHSV {
        get {
            Color hsv = new Color();
            Color.RGBToHSV(activeTheme, out hsv.r, out hsv.g, out hsv.b);
            return hsv;
        }
    }

    public Image border;
    public SpriteRenderer background;

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
        border.color = activeTheme;
        background.color = activeTheme;
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

    void SetRainbow() {
        rainbowTime += Time.deltaTime * rainbowSpeedMultipler;
        themes[themes.Length - 1] = Color.HSVToRGB(rainbowTime, 1, 1);
        if (rainbowTime >= 1) {
            rainbowTime = 0;
        }
    }
}
