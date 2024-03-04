using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public CasinoNavigator casinoNavigator;
    public int targetCameraIndex = 0; // Index of the camera to switch to

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        // Check if the camera switcher script is assigned
        if (casinoNavigator != null)
        {
            // Call the camera switch function with the specified target camera index
            casinoNavigator.SwitchToCamera(targetCameraIndex);
        }
        else
        {
            Debug.LogError("casinoNavigator reference not set in Door script!");
        }
    }
}
