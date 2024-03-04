using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CasinoNavigator : MonoBehaviour
{
    public Camera[] cameras; // Array to hold all your cameras
    private int currentCameraIndex = 0;
    public GameObject lobbyButton;

    // Start is called before the first frame update
    void Start()
    {
        // Disable all cameras except the first one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToLobby()
    {
        // Disable the current camera
        cameras[currentCameraIndex].gameObject.SetActive(false);

        // Set the current camera index to 0
        currentCameraIndex = 0;

        // Enable camera 0
        cameras[currentCameraIndex].gameObject.SetActive(true);
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
