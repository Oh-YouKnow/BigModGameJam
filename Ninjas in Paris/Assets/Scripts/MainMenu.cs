using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //loads game scene
    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        Application.Quit();
    }

    //Used to go back to mainmenu from Lose/Win Screens
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
