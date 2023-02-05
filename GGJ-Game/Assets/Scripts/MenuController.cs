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
    public GameObject globalPrefab;
    public static GameObject GlobalSettings;

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

    private static bool init = true;
    private static int selected = 0;

    void Start()
    {
        if (GlobalSettings == null)
        {
            GlobalSettings = Instantiate(globalPrefab);
            GlobalSettings.name = "GlobalSettings";
            DontDestroyOnLoad(GlobalSettings);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && GlobalSettings != null && SceneManager.GetActiveScene().buildIndex > 0 && GameObject.Find("PlayerObject").GetComponent<PlayerController>().player.getHealth() > 0)
        {
            bool isPaused = (bool)Variables.Object(GlobalSettings).Get("IsPaused");
            if (isPaused)
            {
                Variables.Object(GameObject.Find("GlobalSettings")).Set("IsPaused", false);
                mainPanel.SetActive(false);
            }
            else
            {
                Variables.Object(GameObject.Find("GlobalSettings")).Set("IsPaused", true);
                mainPanel.SetActive(true);
            }
        }
        else
        {
            Debug.Log(GlobalSettings);
            Debug.Log(SceneManager.GetActiveScene().buildIndex);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.GetButtonDown("UP"))
            {
                selected = (selected - 1) % 4;
                if (selected < 0)
                    selected = 3 + selected;
            }
            if (Input.GetButtonDown("DOWN"))
            {
                selected = (selected + 1) % 3;
            }
            Debug.Log("Selected: " + selected);
            GameObject.Find("MenuCursor").transform.position = GameObject.Find("Cursor " + selected).transform.position;

            if (Input.GetButtonDown("Submit"))
            {
                if (selected == 0)
                    onPlayButtonClick();
                else if (selected == 1)
                    onSettingsPressed();
                else if (selected == 2)
                    quitGame();
            }
        }

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
        StartCoroutine(nextScene());
    }

    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator nextScene()
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

    public void returnToMenu()
    {
        StartCoroutine(returnToMenuFade());
    }
    IEnumerator returnToMenuFade()
    {
        Color currColor = fadePanel.GetComponent<Image>().color;
        float fadeAmount;

        while (fadePanel.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = (float)(currColor.a + (0.7 * Time.deltaTime));
            currColor = new Color(currColor.r, currColor.g, currColor.b, fadeAmount);
            fadePanel.GetComponent<Image>().color = currColor;
            yield return null;
        }

        SceneManager.LoadScene(0);
    }
}
