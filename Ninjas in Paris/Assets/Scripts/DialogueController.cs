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

    [SerializeField] GameObject[] disabled;

    [SerializeField] AudioClip gasp;

    private GameObject currentDialogue;

    private int dialogueState = 0;

    private int dialoguePath;
    // Start is called before the first frame update
    void Start()
    {
        createDialogue("Sampletext", "Kanye", kanyeSprite1, kanyeSprite2);

        foreach(GameObject obj in disabled) {
            obj.SetActive(false);
        }
    }

    private void createDialogue(string text, string name, Sprite image1, Sprite image2) {
        if (currentDialogue) Destroy(currentDialogue);
        currentDialogue = Instantiate(DialogueBox, transform);
        currentDialogue.GetComponent<DialogueBox>().createDialogueBox(image1, image2, text, name);

    }

    private void Update() {
        dialoguePath = Random.Range(1, 5);
        if (Input.GetButtonDown("Attack") || dialogueState == 0){
            switch (dialogueState) {
                case 0:
                    switch (dialoguePath) {
                        case 1:
                            createDialogue("I wanna kill and eat homeless people", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                        case 2:
                            createDialogue("I like some of the gaga songs. What the fuck does she know about cameras?", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                        case 3:
                            createDialogue("I hate when I'm on a plane and I wake up with a water bottle next to me like oh great now I gotta be responsible for this water bottle", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                        case 4:
                            createDialogue("People say money can’t buy happiness, but it sure as hell bought me a private island and I’m pretty happy on it", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                    }
                    break;
                case 1:
                    switch (dialoguePath) {
                        case 1:
                            createDialogue("Erm, based based police? I'd like to issue a report", "Jones", jonesSprite1, jonesSprite2);
                            break;
                        case 2:
                            createDialogue("OMG go off bestie", "Jones", jonesSprite1, jonesSprite2);
                            break;
                        case 3:
                            createDialogue("Yass Queen. Wait...", "Jones", jonesSprite1, jonesSprite2);
                            break;
                        case 4:
                            createDialogue("I just shit myself", "Jones", jonesSprite1, jonesSprite2);
                            break;
                    }
                    
                    break;
                case 2:
                    switch (dialoguePath) {
                        case 1:
                            createDialogue("Wazzah! Ninjas??? In Paris????", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                        case 2:
                            createDialogue("Holy Crap! Ninjas here?", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                        case 3:
                            createDialogue("I actually expected this definetly", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                        case 4:
                            createDialogue("AAAHHHH NINJASSSS AAAHHHH", "Kanye", kanyeSprite1, kanyeSprite2);
                            break;
                    }


                    currentDialogue.GetComponent<AudioSource>().clip = gasp;
                    currentDialogue.GetComponent<AudioSource>().Play();
                    break;
                default:
                    startGame();
                    break;
            }
            dialogueState++;
        }
    }

    private void startGame() {
        foreach (GameObject obj in disabled) {
            obj.SetActive(true);
        }
        this.gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SpawnTesting");
    }
}
