using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

[System.Serializable]
public struct DialogueFrame
{
    public string name;
    public Sprite portrait;
    public string message;
    [Tooltip("The time it takes between each character")]
    public float writeWaitTime;
    public UnityEvent onWriteEvent;
}

public class DialogueController : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Image portrait;

    [Header("Dialogue")]
    public UnityEvent onFinishedChatting;
    public DialogueFrame[] dialogues;
    public int dialogueIndex = 0;
    private Coroutine writeRoutine;

    public bool inConversation = false;
    private bool writing = false;
    
    public CasinoNavigator casinoNavigator;

    // Start is called before the first frame update
    void Start()
    {
        dialogueUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        casinoNavigator.inConversation = inConversation;

        if (inConversation == false)
        {
            dialogueIndex = 0;
            nameText.text = "";
            dialogueText.text = "";

        }
    }

    public void OnMouseDown()
    {
        if (inConversation == false)
        {
            BeginConversation();
            GoNext();
        }
        else
        {
            GoNext();
        }
    }

    public void BeginConversation()
    {
        dialogueIndex = 0;
        dialogueUI.SetActive(true);
        inConversation = true;
    }

    public void EndConversation()
    {
        dialogueUI.SetActive(false);
        inConversation = false;
    }

    public void GoNext()
    {
        /*if (writing)
        {
            FinishLine();
            return;
        }*/

        if(dialogueIndex >= dialogues.Length)
        {
            EndConversation();
        }
        else
        {
            DisplayDialoguePiece(dialogueIndex);
        }

        dialogueIndex++;
    }

    private void DisplayDialoguePiece(int i)
    {
        dialogues[i].onWriteEvent.Invoke();
        Debug.Log("Working");
        nameText.text = dialogues[i].name;
        dialogueText.text = dialogues[i].message;
        //portrait = dialogues[i].portrait;
        //if (writeRoutine != null) StopCoroutine(writeRoutine);
        //writeRoutine = StartCoroutine(WriteRoutine(dialogues[i].message, dialogues[i].writeWaitTime));
    }

    /*private IEnumerator WriteRoutine(string msg, float waitTime)
    {
        DialogueManager.Main.characterText.text = "";
        int i = 0;
        writing = true;
        while(i < msg.Length)
        {
            yield return new WaitForSeconds(waitTime);
            DialogueManager.Main.characterText.text += msg[i];
            i++;
        }
        writing = false;
    }*/
}
