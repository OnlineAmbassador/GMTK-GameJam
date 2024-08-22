using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    [SerializeField] KeyCode gameRestartKey = KeyCode.R;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(gameRestartKey))
        {
           
        }
    }
}
