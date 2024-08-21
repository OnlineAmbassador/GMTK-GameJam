using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }
    /* public void MainFromDeath()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2); 
       Time.timeScale = 1f;
    }*/

     public void QuitGame()
    {
        Debug.Log ("Going off the rails? CHUUUUUU CHUUUUU goodbye");
        Application.Quit();
    }
}