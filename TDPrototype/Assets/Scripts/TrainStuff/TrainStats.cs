using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStats : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public void Start()
    {
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            health -= other.GetComponent<EnemyController>().damage;
            //Destroy(other.gameObject);
            if (health <= 0)
            {
                //GameObject.FindFirstObjectByType<EnemySpawnerScript>().addMoney();

                //health = 100000000;
                //Destroy(gameObject);
                GameObject.FindFirstObjectByType<TrainGameController>().DestroyCart(1);
                Debug.Log("Oi I'm Pissed");
            }
        }
    }

}
