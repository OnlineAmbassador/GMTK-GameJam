using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


//This Class is meant to be the behaviour controller for the gun, handles targetting and spawning projectiles? slash raycast lines.
public class TeslaScript : MonoBehaviour
{
    public float lookTime = 0;
    public float fireRate = .5f;
    public float fireTime = .5f;
    public float damage = 45;
    Quaternion toRotation;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //target
        //find enemy
        lookTime += Time.deltaTime;
        fireTime -= Time.deltaTime;
        if (fireTime < 0)
        {
            fireTime = fireRate;
            var b = Instantiate(bullet, transform.position, transform.rotation);
            b.transform.localScale = new Vector3(15, 15, 15);
            b.GetComponent<BulletScript>().damage = 20;
            b.GetComponent<BulletScript>().speed = 0;

        }

    }
}
