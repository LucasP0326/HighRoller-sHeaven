using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyObject : MonoBehaviour
{
    public string scene1; // Name of the scene to load

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
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene1);
    }
}
