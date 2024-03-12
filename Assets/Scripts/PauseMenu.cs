using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false;
    public string LobbyScene;
    public string MainMenu;

    public GameObject pauseMenuUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameIsPaused)
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
    
    public void LoadLobby()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(LobbyScene);
    }

    public void QuitGame()
    {
        Debug.Log("Exit Game");
        UnityEngine.SceneManagement.SceneManager.LoadScene(MainMenu);
    }
}
