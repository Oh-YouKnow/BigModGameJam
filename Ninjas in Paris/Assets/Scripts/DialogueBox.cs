using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] GameObject dialogueImage;
    
    [SerializeField] GameObject dialogueName;
    [SerializeField] GameObject Dialogue;

    private string finishedText;
    private float textTimer = 0;
    private float imageTimer = 0;
    Sprite dialogueImage1;
    Sprite altdialogueImage2;

    // Update is called once per frame
    public void createDialogueBox(Sprite image1, Sprite image2, string text, string name) {

        
        Dialogue.GetComponent<TextMeshProUGUI>().text = "";
        finishedText = text;

        dialogueImage1 = image1;
        altdialogueImage2 = image2;
        dialogueName.GetComponent<TextMeshProUGUI>().text = name;
    }

    private void Update() {
        if (finishedText != "" && textTimer < 0) {
            Dialogue.GetComponent<TextMeshProUGUI>().text += finishedText.Substring(0, 1);
            finishedText = finishedText.Substring(1);
            textTimer = .03f;
        }
        if (imageTimer < 0) {
            dialogueImage.GetComponent<SpriteRenderer>().sprite = altdialogueImage2;
            imageTimer = .6f;
            Debug.Log("reset");
        }
        else if (imageTimer < .3f) {
            dialogueImage.GetComponent<SpriteRenderer>().sprite = dialogueImage1;
            
        }
        //dialogueImage.GetComponent<SpriteRenderer>().sprite[1];
        textTimer -= Time.deltaTime;
        imageTimer -= Time.deltaTime;
    }
}
