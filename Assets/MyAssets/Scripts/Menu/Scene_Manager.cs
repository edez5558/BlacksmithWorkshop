using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene("MainGame");
    }

    public void LoadInstructions(){
        SceneManager.LoadScene("Instructions");
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadScore(){
        SceneManager.LoadScene("Score");
    }
    
    public void ExitGame(){
        Application.Quit();
    }
}
