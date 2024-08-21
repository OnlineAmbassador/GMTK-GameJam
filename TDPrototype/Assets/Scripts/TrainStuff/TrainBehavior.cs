using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject NearestEnemy()
    {
        GameObject[] value = GameObject.FindGameObjectsWithTag("Enemy");
        float mindist = Mathf.Infinity;
        GameObject minem = null;
        for (int i = 0; i < value.Length; i++)
        {
            float tesdis = Vector3.Distance(value[i].transform.position, transform.position);
            if (tesdis < mindist)
            {
                mindist = tesdis;
                minem = value[i];
            }
        }

        return minem;

    }



}
