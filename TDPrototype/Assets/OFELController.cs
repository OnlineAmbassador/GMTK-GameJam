using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OFELController : MonoBehaviour
{
    public static OFELController instance { get; private set; }
    void Awake()
    {
        // Check if an instance of GameManager already exists
        if (instance == null)
        {
            // If not, set it to this instance
            instance = this;
            // Make this instance persistent
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            // If an instance already exists and it's not this one, destroy this one
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
