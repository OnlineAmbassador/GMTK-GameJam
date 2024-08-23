using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startButtonController : MonoBehaviour
{
    public static Light buttonlight;
    [SerializeField] Light l;
    void Start()
    {
        buttonlight = l;
        buttonlight.intensity = 0;
    }
}
