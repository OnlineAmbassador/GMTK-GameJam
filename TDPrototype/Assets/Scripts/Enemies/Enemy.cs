using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{

    public int nodeIndex;
    public float maxHealth;
    public float health;
    public float speed;
    public int id;

    public void Init()
    {
        health = maxHealth;
        transform.position = GameLoopManager.nodePositions[0];
        nodeIndex = 0;
    }
}
