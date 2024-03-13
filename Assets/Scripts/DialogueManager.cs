using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Simply a singleton for all DialoguePopupController scripts to reference
 * 
 * 
 * Caden Henderson
 * 1/26/2024
 */

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Main;
    public TMP_Text characterNameText;
    public TMP_Text characterText;
    public Image characterPortrait;

    [HideInInspector] public Image dialogueBackdrop;

    private void Start()
    {
        //set up singleton and hide
        if (Main != null) Destroy(gameObject);
        Main = this;

        //hide
        dialogueBackdrop = gameObject.GetComponent<Image>();
        gameObject.SetActive(false);
    }
}
