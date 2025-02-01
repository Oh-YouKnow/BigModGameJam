using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{



    [SerializeField] GameObject DialogueBox;
    [SerializeField] Sprite kanyeSprite1;
    [SerializeField] Sprite jonesSprite1;
    [SerializeField] Sprite kanyeSprite2;
    [SerializeField] Sprite jonesSprite2;

    [SerializeField] GameObject[] disabledGameObjects;

    private GameObject currentDialogue;

    private int dialogueState = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in disabledGameObjects) {
            obj.SetActive(false);
        }
        createDialogue("Sampletext", "Kanye", kanyeSprite1, kanyeSprite2);

    }

    private void createDialogue(string text, string name, Sprite image1, Sprite image2) {

        if (currentDialogue) Destroy(currentDialogue);
        currentDialogue = Instantiate(DialogueBox, transform);
        currentDialogue.GetComponent<DialogueBox>().createDialogueBox(image1, image2, text, name);
    }

    private void Update() {
        if (Input.GetButtonDown("Attack") || dialogueState == 0){
            switch (dialogueState) {
                case 0:
                    createDialogue("I wanna kill and eat homeless people", "Kanye", kanyeSprite1, kanyeSprite2);
                    break;
                case 1:
                    createDialogue("Erm, based alert", "Jones", jonesSprite1, jonesSprite2);
                    break;
                case 2:
                    createDialogue("Wazzah! Ninjas??? In Paris????", "Kanye", kanyeSprite1, kanyeSprite2);
                    break;

                case 3:
                    startGame();
                    break;
            }
            dialogueState++;
        }
    }

    public void startGame() {
        Destroy(currentDialogue);
        foreach(GameObject obj in disabledGameObjects){
            obj.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }
}
