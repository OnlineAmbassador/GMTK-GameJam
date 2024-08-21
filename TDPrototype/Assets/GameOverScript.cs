using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) {
            EnemySpawnerScript.numEnemies = 0;
            string currentSceneName = SceneManager.GetActiveScene().name;
            //SceneManager.UnloadSceneAsync(currentSceneName);
            SceneManager.LoadScene(currentSceneName);
            
        }
    }
}
