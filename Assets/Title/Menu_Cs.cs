using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Cs : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private GameObject volumeText;
    [SerializeField] private GameObject levelSelectButton;
    [SerializeField] private GameObject level1;

    [SerializeField] InputActionAsset inputActions;

    private Music_Cs music;
    private FadeEffect fade;
    public FadeEffectUI_Cs fadeUI;

    public int selected;

    private bool startingLevel;

    void Start()
    {
        fade = GameObject.Find("FadeEffect").GetComponent<FadeEffect>();
        music = GameObject.Find("MusicPlayer").GetComponent<Music_Cs>();
        volumeSlider.GetComponent<Slider>().value = music.volume;
        startingLevel = false;

        inputActions.FindActionMap("UI").FindAction("Cancel").performed += (InputAction.CallbackContext context) =>
        {
            selected = 0;
            ChangeMenu();
        };
    }
    
    void Update()
    {
        if (startingLevel) return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            selected = 0;
            ChangeMenu();
        }
        volumeText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(music.volume*100) + "%";
    }

    public void ChangeVolume()
    {
        music.volume = volumeSlider.GetComponent<Slider>().value;
    }

    public void LevelSelect()
    {
        selected = 1;

    }

    public void SettingsMenu()
    {
        selected = 2;
    }

    public void ChangeMenu()
    {
        List<Transform> menus = new List<Transform>()
        {
            mainMenu.transform, levelSelect.transform, settingsMenu.transform
        };

        for (int i = 0; i < menus.Count; i++)
        {
            if (i == selected)
            {
                menus[i].gameObject.SetActive(true);
                continue;
            }

            menus[i].gameObject.SetActive(false);
        }

        if (selected == 1)
        {
            EventSystem.current.SetSelectedGameObject(level1.gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(levelSelectButton.gameObject);
        }
    }

    public void LoadLevel(int level)
    {
        startingLevel = true;
        StartCoroutine(LevelTransition(level));
    }

    IEnumerator LevelTransition(int level)
    {
        StartCoroutine(fade.FadeOut());
        StartCoroutine(fadeUI.FadeOut());
        yield return new WaitForSeconds(1/0.8f);
        SceneManager.LoadScene(level);
    }
}
