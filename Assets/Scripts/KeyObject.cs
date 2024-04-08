using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyObject : MonoBehaviour
{
    public string scene1; // Name of the scene to load
    public string scene2; // Name of the scene to load
    public bool inConversation;
    public DialogueController[] characters;
    public bool cardTable;
    public GameObject cardGameSelect;

    // Start is called before the first frame update
    void Start()
    {
        cardGameSelect.SetActive(false);
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
    }

    void OnMouseDown()
    {
        if (inConversation == false && Time.timeScale != 0 && cardTable == true)
        {
            cardGameSelect.SetActive(true);
        }
        if (inConversation == false && Time.timeScale != 0 && cardTable == false)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene1);
        }
    }

    public void LoadNormal()
    {
        if (inConversation == false && Time.timeScale != 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene1);
        }
    }

    public void LoadBlackJack()
    {
        if (inConversation == false && Time.timeScale != 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene2);
        }
    }
}
