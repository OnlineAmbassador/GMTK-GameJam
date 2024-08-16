using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;
public class GameLoopManager : MonoBehaviour
{
    public static Vector3[] nodePositions;
    private static Queue<Enemy> enemiesToRemove;
    private static Queue<int> enemyIDsToSummon;
    public Transform nodeParent;
    public bool loopShouldEnd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyIDsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<Enemy>();
        EntitySummoner.Init();

        nodePositions = new Vector3[nodeParent.childCount];

        for(int i = 0; i < nodePositions.Length; i++)
        {
            nodePositions[i] = nodeParent.GetChild(i).position;
        }

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
        // InvokeRepeating("RemoveTest", 0f, 0.5f);
    }

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
    }

    // void RemoveTest()
    // {
    //     if(EntitySummoner.enemiesInGame.Count > 0)
    //     {
    //         EntitySummoner.RemoveEnemy(EntitySummoner.enemiesInGame[Random.Range(0, EntitySummoner.enemiesInGame.Count)]);
    //     }
    // }

    IEnumerator GameLoop()
    {
        while(loopShouldEnd == false)
        {
            //Spawn Enemies

            if (enemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < enemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(enemyIDsToSummon.Dequeue());
                }
            }

            //spawn towers

            //move enemies
            NativeArray<Vector3> NodesToUse = new NativeArray<Vector3>(nodePositions, Allocator.TempJob);
            NativeArray<int> NodeIndices = new NativeArray<int>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            NativeArray<float> EnemySpeeds = new NativeArray<float>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoner.enemiesInGameTransform.ToArray(), 2);

            for(int i=0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                EnemySpeeds[i] = EntitySummoner.enemiesInGame[i].speed;
                NodeIndices[i] = EntitySummoner.enemiesInGame[i].nodeIndex;
            }

            MoveEnemiesJob moveJob = new MoveEnemiesJob
            {
                nodePositions = NodesToUse,
                enemySpeed = EnemySpeeds,
                nodeIndex = NodeIndices,
                deltaTime = Time.deltaTime
            };

            JobHandle moveJobHandle = moveJob.Schedule(EnemyAccess);
            moveJobHandle.Complete();

            for(int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                EntitySummoner.enemiesInGame[i].nodeIndex = NodeIndices[i];

                if(EntitySummoner.enemiesInGame[i].nodeIndex == nodePositions.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoner.enemiesInGame[i]);
                }
            }

            NodesToUse.Dispose();
            EnemySpeeds.Dispose();
            NodeIndices.Dispose();
            EnemyAccess.Dispose();

            //proc towers

            //effects

            //damage

            //kill enemies

             if(enemiesToRemove.Count > 0)
            {
                for(int i = 0; i < enemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(enemiesToRemove.Dequeue());
                }
            }

            //remove towers
            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int id)
    {
        enemyIDsToSummon.Enqueue(id);
    }

    public static void EnqueueEnemyToRemove(Enemy enemyToRemove)
    {
        enemiesToRemove.Enqueue(enemyToRemove);
    }

}

public struct MoveEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<int> nodeIndex;
    [NativeDisableParallelForRestriction]
    public NativeArray<float> enemySpeed;
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> nodePositions;
    public float deltaTime;
    public void Execute(int index, TransformAccess transform)
    {
        if(nodeIndex[index] < nodePositions.Length)
        {

        
        Vector3 postionToMoveTo = nodePositions[nodeIndex[index]];
        transform.position = Vector3.MoveTowards(transform.position, postionToMoveTo, enemySpeed[index] * deltaTime);
        

        if(transform.position == postionToMoveTo)
        {
            nodeIndex[index]++;
        }
        }
    }
}
