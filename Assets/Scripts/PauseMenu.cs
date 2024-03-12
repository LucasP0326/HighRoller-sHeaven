using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;


    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameIsPaused)
            {
                Debug.Log("Resume");
                Resume();
            }
            else
            {
                Debug.Log("Pause");
                Pause();
            }
        }
    }
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
    }

    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
    }
    
}
