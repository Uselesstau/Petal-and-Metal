using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Cs : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private GameObject volumeText;
    
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
                menus[i].localScale = new Vector3(1, 1, 1);
                continue;
            }

            menus[i].localScale = Vector3.zero;
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
