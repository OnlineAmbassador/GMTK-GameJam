using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


//This Class is meant to be the behaviour controller for the gun, handles targetting and spawning projectiles? slash raycast lines.
public class GunScript : MonoBehaviour
{
    public GameObject head;
    GameObject lastEnemy;
    public float lookTime = 0;
    public float fireRate = .5f;
    public float fireTime = .5f;
    Quaternion toRotation;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        lastEnemy = null;
    }

    // Update is called once per frame
    void Update()
    {
        //target
        //find enemy
        if (lookTime > 1f)
        {
            GameObject nearestEnemy = NearestEnemy();

            if (nearestEnemy != null && lastEnemy != nearestEnemy)
            {
                lastEnemy = nearestEnemy;
            }
            lookTime = 0;
        }
        lookTime += Time.deltaTime;
        fireTime -= Time.deltaTime; 
        if (lastEnemy != null)
        {
            head.transform.LookAt(lastEnemy.transform.position);
            if (fireTime < 0)
            {
                fireTime = fireRate;
                Instantiate(bullet, head.transform.position + head.transform.forward * 6, head.transform.rotation);

            }
        }

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
