using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button Play;
    public Button Quit;
    public string LevelLoad;
  
    void Start()
    {
        // Attach the button click listener
        if (Play != null)
        {
            Play.onClick.AddListener(TransitionToScene);
        }

        if (Quit != null)
        {
            Application.Quit();
        }

    }
    void TransitionToScene()
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
}
