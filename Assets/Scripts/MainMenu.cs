using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button Play;
    public string LevelLoad;
    public GameObject mainMenu;
    public GameObject introPanel;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public GameObject playButton2;
  
    void Start()
    {
        introPanel.SetActive(false);
        // Attach the button click listener
        /*if (Play != null)
        {
            Play.onClick.AddListener(TransitionToScene);
        }*/
        Time.timeScale = 1;
    }
    public void TransitionToScene()
    {
        // Check if the sceneToLoad reference is not null
        if (!string.IsNullOrEmpty(LevelLoad))
        {
            // Load the scene by its name
            UnityEngine.SceneManagement.SceneManager.LoadScene(LevelLoad);
        }
        else
        {
            Debug.LogWarning("Scene to load is not assigned.");
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlaySequence()
    {
        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {
        introPanel.SetActive(true);
        mainMenu.SetActive(false);
        text1.enabled = false;
        text2.enabled = false;
        text3.enabled = false;
        playButton2.SetActive(false);
        yield return new WaitForSeconds(1f);
        text1.enabled = true;
        yield return new WaitForSeconds(3f);
        text2.enabled = true;
        yield return new WaitForSeconds(3f);
        text3.enabled = true;
        yield return new WaitForSeconds(3f);
        playButton2.SetActive(true);

        yield return null;
    }
}
