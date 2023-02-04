using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MenuController : MonoBehaviour
{

    [Header("Global Settings")]
    [InspectorName("Global Settings")]
    public GameObject GlobalSettings;

    [Header("Menu Settings")]
    [InspectorName("Fade Panel")]
    public GameObject fadePanel;
    [InspectorName("Audio Source")]
    public AudioSource audioSoruce;
    [InspectorName("Play Sound")]
    public AudioClip selectSound;
    [InspectorName("Main Panel")]
    public GameObject mainPanel;
    [InspectorName("Settings Panel")]
    public GameObject settingsPanel;

    void Start()
    {
        DontDestroyOnLoad(GlobalSettings);
    }

    // Update is called once per frame
    void Update()
    {}

    public void onMouseSenseChange()
    {
        float value = GameObject.Find("ZoomSense").GetComponent<Slider>().value;
        Variables.Object(GlobalSettings).Set("ZoomSensitivity", value);
    }
    public void onVolumeChange()
    {
        float value = GameObject.Find("VolumeLevel").GetComponent<Slider>().value;
        Variables.Object(GlobalSettings).Set("Volume", value);
        audioSoruce.volume = value;
    }
    public void onBackPressed()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    public void onSettingsPressed()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void onPlayButtonClick()
    {
        audioSoruce.clip = selectSound;
        audioSoruce.Play();
        StartCoroutine(fade());
    }

    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator fade()
    {
        Color currColor = fadePanel.GetComponent<Image>().color;
        float fadeAmount;
        
        while(fadePanel.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = (float)(currColor.a + (0.7 * Time.deltaTime));
            currColor = new Color(currColor.r, currColor.g, currColor.b, fadeAmount);
            fadePanel.GetComponent<Image>().color = currColor;
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
