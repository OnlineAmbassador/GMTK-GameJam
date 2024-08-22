using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EnemySpawnerScript : MonoBehaviour
{
   
    float timer = 0f;

    public static int totalMoney;
    public bool waitForContinue;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public static int numEnemies;
    public AudioSource yay;
    public static int waveNumber = 0;
    public GameObject win;
    [SerializeField] public List<GameObject> spawns = new List<GameObject>();
    [SerializeField] public List<Wave> waves = new List<Wave>();
    public void addMoney()
    {
        //If an enemy dies we add some money based on wave
        totalMoney += waves[waveNumber - 1].reward;
        numEnemies--;

    }
    // Start is called before the first frame update
    void Start()
    {
        totalMoney = 20;
        for(int i = 0; i < 10; i++)
        {
            waves.Add(makeWave(i+1));
        }
        waitForContinue = true;
    }

    Wave makeWave(int num)
    {
        Wave w = new Wave();
        w.enemyCount = 5*num;
        w.lastFight = true;
        w.timeForSpawns = 1f;
        w.reward = 1;
        w.enemyType = enemy1;
        return w;
    }

    // Update is called once per frame
    void Update()
    {
        if(waveNumber>=waves.Count && numEnemies <= 0)
        {
            waveNumber = 0;
            Debug.Log("Switching Levels" + SceneManager.GetActiveScene().buildIndex.ToString());
            GetComponent<MainMenu>().PlayGame();
        }
        else if (!waitForContinue)
        {
            Wave cwave = waves[waveNumber];
            timer += Time.deltaTime;
            if (timer > cwave.timeForSpawns)
            {
                if (!cwave.lastFight)
                {
                    spawnEnems(cwave);
                    waveNumber++;
                    timer = 0f;
                }
                else
                {
                    spawnEnems(cwave);
                    waveNumber++;
                    waitForContinue = true;
                }
            }
        }
        else if (numEnemies <= 0)
        {
            if (!PlayerMovement.isBigMode)
            {
                PlayerMovement.SwitchPerspective();
                FrontCartScript fcs = GameObject.FindObjectOfType<FrontCartScript>();
                if (fcs != null)
                {
                    fcs.health = fcs.maxHealth;
                    fcs.maxHealth += 10;
                }
                yay.Play();
            }
        }
    }


    public void spawnEnems(Wave wav)
    {
        Debug.Log("Spawning");

        numEnemies = wav.enemyCount;
        for (int i = 0; i < wav.enemyCount; i++)
        {
            GameObject g = Instantiate(wav.enemyType, spawns[((int)(Random.value * spawns.Count))].transform.position, Quaternion.identity);
            g.GetComponent<EnemyController>().target = findNearestTrain(g);
            g.GetComponent<NavMeshAgent>().speed = (float)(waveNumber * 3+5);
            g.GetComponent<NavMeshAgent>().acceleration = (float)(waveNumber * 5 +10);
            g.GetComponent<EnemyController>().hp = (float)(waveNumber * 5 + 5);
        }
    }
    public void nextWave()
    {
        waitForContinue = false;
    }
    public GameObject findNearestTrain(GameObject g) 
    {
        GameObject[] value = GameObject.FindGameObjectsWithTag("train");
        float mindist = Mathf.Infinity;
        GameObject minem = null;
        for (int i = 0; i < value.Length; i++)
        {
            float tesdis = Vector3.Distance(value[i].transform.position, g.transform.position);
            if (tesdis < mindist)
            {
                mindist = tesdis;
                minem = value[i];
            }
        }
        return minem;
    }
}
public struct Wave
{
    public int enemyCount;
    public int reward;
    public float timeForSpawns;
    public bool lastFight;
    public GameObject enemyType;
}