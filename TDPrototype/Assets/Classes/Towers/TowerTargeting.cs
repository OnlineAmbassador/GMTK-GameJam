using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;
using UnityEngine;

public class TowerTargeting
{
    public enum targetType
    {
        First,
        Last,
        Close
    }
    public static Enemy GetTarget(TowerBehavior currentTower, targetType targetMethod)
    {
        Debug.Log(currentTower);
        // Collider[] enemiesInRange = Physics.OverlapSphere(currentTower.transform.position, currentTower.range, currentTower.EnemiesLayer);

        if (currentTower == null)
        {
            Debug.Log("currentTower is null! NOOO!!!!");
            return null;
        }

        if (currentTower.EnemiesLayer == 0)
        {
            Debug.LogError("EnemiesLayer is not set on currentTower!");
            return null;
        }

        Collider[] enemiesInRange = Physics.OverlapSphere(currentTower.transform.position, currentTower.range, currentTower.EnemiesLayer);
        Debug.Log("Enemies in range: " + enemiesInRange.Length);

        if (enemiesInRange.Length == 0)
        {
            Debug.Log("No enemees");
            return null;
        }



        NativeArray<EnemyData> enemiesToCalculate = new NativeArray<EnemyData>(enemiesInRange.Length, Allocator.TempJob);
        NativeArray<Vector3> nodePositions = new NativeArray<Vector3>(GameLoopManager.nodePositions, Allocator.TempJob);
        NativeArray<float> nodeDistances = new NativeArray<float>(GameLoopManager.nodeDistances, Allocator.TempJob);
        NativeArray<int> enemyToIndex = new NativeArray<int>(new int[] { -1 }, Allocator.TempJob);
        int enemyIndexToReturn = -1;



        for (int i = 0; i < enemiesToCalculate.Length; i++)
        {
            Enemy currentEnemy = enemiesInRange[i].transform.parent.GetComponent<Enemy>();
            if (currentEnemy != null)
            {
                int enemyIndexInList = EntitySummoner.enemiesInGame.FindIndex(x => x == currentEnemy);
                enemiesToCalculate[i] = new EnemyData(currentEnemy.transform.position, currentEnemy.nodeIndex, currentEnemy.health, enemyIndexInList);
            }

        }

        SearchForEnemy EnemySearchJob = new SearchForEnemy
        {
            _enemiesToCalculate = enemiesToCalculate,
            _nodePositions = nodePositions,
            _nodeDistances = nodeDistances,
            _enemyToIndex = enemyToIndex,
            compareValue = Mathf.Infinity,
            targetingType = (int)targetMethod,
            towerPosition = currentTower.transform.position
        };

        switch ((int)targetMethod)
        {
            case 0: //First
                EnemySearchJob.compareValue = Mathf.Infinity;
                break;
            case 1: //Last
                EnemySearchJob.compareValue = Mathf.NegativeInfinity;
                break;
            case 2: //Close
                goto case 0;
        }

        JobHandle dependency = new JobHandle();
        JobHandle SearchJobHandle = EnemySearchJob.Schedule(enemiesToCalculate.Length, dependency);

        SearchJobHandle.Complete();

        enemyIndexToReturn = enemiesToCalculate[enemyToIndex[0]].enemyIndex;
        enemiesToCalculate.Dispose();
        nodePositions.Dispose();
        nodeDistances.Dispose();
        enemyToIndex.Dispose();

        if (enemyIndexToReturn == -1)
        {
            return null;
        }
        return EntitySummoner.enemiesInGame[enemyIndexToReturn];
    }

    struct EnemyData
    {
        public EnemyData(Vector3 position, int nodeindex, float hp, int enemyindex)
        {
            enemyPosition = position;
            nodeIndex = nodeindex;
            health = hp;
            enemyIndex = enemyindex;
        }

        public int nodeIndex;
        public float health;
        public Vector3 enemyPosition;
        public int enemyIndex;

    }

    struct SearchForEnemy : IJobFor
    {

        [ReadOnly] public NativeArray<EnemyData> _enemiesToCalculate;
        [ReadOnly] public NativeArray<Vector3> _nodePositions;
        [ReadOnly] public NativeArray<float> _nodeDistances;
        [NativeDisableParallelForRestriction] public NativeArray<int> _enemyToIndex;
        public Vector3 towerPosition;
        public float compareValue;
        public int targetingType;

        public void Execute(int index)
        {
            float currentEnemyDistanceToEnd = 0;
            float distanceToEnemy = 0;
            switch (targetingType)
            {
                case 0: //First

                    currentEnemyDistanceToEnd = getDistanceToEnd(_enemiesToCalculate[index]);

                    if (currentEnemyDistanceToEnd < compareValue)
                    {
                        _enemyToIndex[0] = index;
                        compareValue = currentEnemyDistanceToEnd;
                    }

                    break;
                case 1: //Last


                    currentEnemyDistanceToEnd = getDistanceToEnd(_enemiesToCalculate[index]);

                    if (currentEnemyDistanceToEnd > compareValue)
                    {
                        _enemyToIndex[0] = index;
                        compareValue = currentEnemyDistanceToEnd;
                    }

                    break;
                case 2:  //Close

                    distanceToEnemy = Vector3.Distance(towerPosition, _enemiesToCalculate[index].enemyPosition);

                    if (distanceToEnemy > compareValue)
                    {
                        _enemyToIndex[0] = index;
                        compareValue = distanceToEnemy;
                    }
                    break;
            }
        }

        private float getDistanceToEnd(EnemyData enemyToEvaluate)
        {
            float finalDistance = Vector3.Distance(enemyToEvaluate.enemyPosition, _nodePositions[enemyToEvaluate.nodeIndex - 1]);

            for (int i = enemyToEvaluate.nodeIndex; i < _nodeDistances.Length; i++)
            {
                finalDistance += _nodeDistances[i];
            }

            return finalDistance;
        }
    }
}
