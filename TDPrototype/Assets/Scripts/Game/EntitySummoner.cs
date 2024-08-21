using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{

    public static List<Enemy> enemiesInGame;
    public static List<Transform> enemiesInGameTransform;
    public static Dictionary<int, GameObject> enemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> enemyObjectPools;
    
    private static bool isInitialized;
    public static void Init()
    {
        if (!isInitialized)
        {
            enemyPrefabs = new Dictionary<int, GameObject>();
            enemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            enemiesInGame = new List<Enemy>();
            enemiesInGameTransform = new List<Transform>();

            EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");
        
            foreach(EnemySummonData enemy in enemies)
            {
                enemyPrefabs.Add(enemy.enemyID, enemy.enemyPrefab);
                enemyObjectPools.Add(enemy.enemyID, new Queue<Enemy>());
            }

            isInitialized = true;
        }
        else
        {
            Debug.Log("ENTITYSUMMONER: THIS CLASS IS ALREADY INITIALIZED");
        }
    }

    public static Enemy SummonEnemy(int enemyID)
    {
        Enemy summonedEnemy = null;
        
        if(enemyPrefabs.ContainsKey(enemyID))
        {
            Queue<Enemy> ReferencedQueue = enemyObjectPools[enemyID];

            if(ReferencedQueue.Count > 0)
            {
                //Dequeue enemy

                summonedEnemy = ReferencedQueue.Dequeue();
                summonedEnemy.Init();
                summonedEnemy.gameObject.SetActive(true);
            }
            else
            {
                //Instantiate new enemy
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyID], GameLoopManager.nodePositions[0], Quaternion.identity);
                summonedEnemy = newEnemy.GetComponent<Enemy>();
            }
        }
        else
        {
            Debug.Log("ENTITYSUMMONER: ENEMY WITH ID OF {enemyID} DOES NOT EXIST!");
            return null;
        }

        enemiesInGameTransform.Add(summonedEnemy.transform);
        enemiesInGame.Add(summonedEnemy);
        summonedEnemy.id = enemyID;
        return summonedEnemy;
    }

    public static void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyObjectPools[enemyToRemove.id].Enqueue(enemyToRemove);
        enemyToRemove.gameObject.SetActive(false);
        enemiesInGameTransform.Remove(enemyToRemove.transform);
        enemiesInGame.Remove(enemyToRemove);
    }
}
