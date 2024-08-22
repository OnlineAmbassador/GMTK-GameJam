using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FrontCartScript : MonoBehaviour
{
    public float health;
    public float maxHealth;
    UIDocument healthbardocument;
    public GameObject gameover;
    public bool enabled = true;

    public void Start()
    {
        enabled = true;
        health = maxHealth;
        healthbardocument = GetComponent<UIDocument>();
    }
    private void Update()
    {
        if (enabled)
        {
            healthbardocument.rootVisualElement.Q<Label>("enemy-counter").text = "Remaining Enemies: " + EnemySpawnerScript.numEnemies;
            healthbardocument.rootVisualElement.Q<Label>("money-counter").text = "Money: " + EnemySpawnerScript.totalMoney;
            healthbardocument.rootVisualElement.Q<Label>("wave-number").text = "Wave: " + EnemySpawnerScript.waveNumber;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (enabled)
        {
            if (other.tag == "Enemy")
            {
                health -= other.GetComponent<EnemyController>().damage;
                other.GetComponent<EnemyController>().damage *= .5f;

                healthbardocument.rootVisualElement.Q<ProgressBar>("train-health").value = health / maxHealth * 100;


                //Destroy(other.gameObject);
                if (health <= 0)
                {
                    enabled = false;
                    GetComponentInChildren<FrontTurretCamera>().enabled = false;
                    Instantiate(gameover);
                }
            }
        }
    }
}
