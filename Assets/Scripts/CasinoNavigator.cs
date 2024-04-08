using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CasinoNavigator : MonoBehaviour
{
    public Camera[] cameras; // Array to hold all your cameras
    public GameObject[] buttons; //Array for lobby buttons
    public int currentCameraIndex = 0;
    public GameObject lobbyButton;
    public bool inConversation;
    public DialogueController[] characters;

    // Start is called before the first frame update
    void Start()
    {
        // Disable all cameras except the first one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        bool allCharactersNotInConversation = true;

        for (int i = 0; i < characters.Length; i++)
        {
            if(characters[i].inConversation == true)
            {
                inConversation = true;
                allCharactersNotInConversation = false;
                break;
            }
        }

        if (allCharactersNotInConversation)
        {
            inConversation = false;
        }

        if (currentCameraIndex == 0)
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
        }
        if (currentCameraIndex == 1)
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
            buttons[2].SetActive(false);
        }
        if (currentCameraIndex == 2)
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(true);
        }
        if (currentCameraIndex == 3)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
        }
    }

    public void ReturnToLobby()
    {
        if(inConversation == false && Time.timeScale != 0)
        {
            // Disable the current camera
            cameras[currentCameraIndex].gameObject.SetActive(false);

            // Set the current camera index to 0
            currentCameraIndex = 0;

            // Enable camera 0
            cameras[currentCameraIndex].gameObject.SetActive(true);
        }
    }

    public void SwitchToCamera(int index)
    {
        // Disable the current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Update the current camera index to the specified index
        currentCameraIndex = index;

        // Enable the new current camera
        cameras[currentCameraIndex].gameObject.SetActive(true);
    }
}
