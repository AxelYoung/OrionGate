using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    Animator animator { get { return GetComponent<Animator>(); } }

    public Animator[] startEvents;

    bool played = false;

    public Sprite[] buttonStates;

    public int currentSetting = 0;
    public Sprite settingsMenu;
    public Image volume;
    public Sprite[] volumeSprites;
    public int currentVolume;
    public Image theme;
    public Sprite[] themeSprites;
    public int currentTheme;
    public Image back;
    public Sprite[] backSprites;

    Image image;

    int activeMenu = 0;
    int activeButton = 0;
    bool verticalPressed = false;
    bool horizontalPressed = false;

    void Start() {
        image = GetComponent<Image>();
    }

    void Update() {
        if (!played) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (activeMenu == 0) {
                    if (activeButton == 0) {
                        animator.enabled = true;
                        animator.SetTrigger("Start");
                        Invoke("PlayStartEvents", 1.85f);
                        played = true;
                    } else {
                        activeMenu = 1;
                        image.sprite = settingsMenu;
                        volume.gameObject.SetActive(true);
                        theme.gameObject.SetActive(true);
                        back.gameObject.SetActive(true);
                    }
                } else {
                    if (currentSetting == 2) {
                        activeMenu = 0;
                        image.sprite = buttonStates[1];
                        volume.gameObject.SetActive(false);
                        theme.gameObject.SetActive(false);
                        back.gameObject.SetActive(false);
                    }
                }
            }
            float vertical = Input.GetAxisRaw("Vertical");
            if (vertical != 0) {
                if (!verticalPressed) {
                    if (activeMenu == 0) {
                        activeButton = activeButton == 0 ? 1 : 0;
                        animator.enabled = false;
                        image.sprite = buttonStates[activeButton];
                    } else {
                        currentSetting += vertical > 0 ? -1 : 1;
                        currentSetting = currentSetting > 2 ? 0 : currentSetting < 0 ? 2 : currentSetting;
                        switch (currentSetting) {
                            case 0:
                                volume.sprite = volumeSprites[currentVolume + (volumeSprites.Length / 2)];
                                theme.sprite = themeSprites[currentTheme];
                                back.sprite = backSprites[0];
                                break;
                            case 1:
                                volume.sprite = volumeSprites[currentVolume];
                                theme.sprite = themeSprites[currentTheme + (themeSprites.Length / 2)];
                                back.sprite = backSprites[0];
                                break;
                            case 2:
                                volume.sprite = volumeSprites[currentVolume];
                                theme.sprite = themeSprites[currentTheme];
                                back.sprite = backSprites[1];
                                break;
                        }
                    }
                    verticalPressed = true;
                }
            } else {
                verticalPressed = false;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal != 0) {
                if (!horizontalPressed) {
                    if (activeMenu == 1) {
                        switch (currentSetting) {
                            case 0:
                                currentVolume += horizontal > 0 ? 1 : -1;
                                currentVolume = currentVolume > 10 ? 10 : currentVolume < 0 ? 0 : currentVolume;
                                GameMaster.instance.volume = currentVolume;
                                volume.sprite = volumeSprites[currentVolume + (volumeSprites.Length / 2)];
                                break;
                            case 1:
                                currentTheme += horizontal > 0 ? 1 : -1;
                                currentTheme = currentTheme > 3 ? 0 : currentTheme < 0 ? 3 : currentTheme;
                                GameMaster.instance.activeThemeIndex = currentTheme;
                                theme.sprite = themeSprites[currentTheme + (themeSprites.Length / 2)];
                                break;
                        }
                        horizontalPressed = true;
                    }
                }
            } else {
                horizontalPressed = false;
            }
        }
    }

    void PlayStartEvents() {
        foreach (Animator eventAnimator in startEvents) {
            eventAnimator.SetTrigger("Start");
        }
        Invoke("StartGame", 2);
    }

    void StartGame() { GameMaster.instance.StartGame(); }
}
