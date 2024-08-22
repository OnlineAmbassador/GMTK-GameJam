using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foots : MonoBehaviour
{
    /*
    [SerializeField] Texture2D cursor;
    [SerializeField] AudioClip[] sounds;
    public AudioSource steps;
    private Coroutine footStepsCoroutine;

    void Start()
    {
        steps = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (PlayerMovement.isBigMode)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (footStepsCoroutine == null)
                {
                    footStepsCoroutine = StartCoroutine(PlayFootsteps());
                }
            }
            else
            {
                if (footStepsCoroutine != null)
                {
                    StopCoroutine(footStepsCoroutine);
                    footStepsCoroutine = null;
                }
            }
        }
        else
        {
            if (footStepsCoroutine != null)
            {
                StopCoroutine(footStepsCoroutine);
                footStepsCoroutine = null;
            }

        }
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            AudioClip clip = sounds[UnityEngine.Random.Range(0, sounds.Length)];
            steps.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }*/
}