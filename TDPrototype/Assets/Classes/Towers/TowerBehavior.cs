using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{

    public LayerMask EnemiesLayer;
    public Enemy target;
    public Transform TowerPivot;

    public float damage;
    public float firerate;
    public float range;
    private float delay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        delay = 1 / firerate;
    }

    public void Tick()
    {
        if(target != null)
        {
            TowerPivot.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
