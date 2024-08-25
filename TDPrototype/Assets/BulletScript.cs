using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float life = 1;
    public float damage = 1;
    public float speed = 300;
    public ParticleSystem bulletParticle;
    // Start is called before the first frame update
    void Start()
    {
        tag = "bullet";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(bulletParticle, transform.position, Quaternion.identity);
            life = -0.1f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed  * Time.deltaTime);
        life -= Time.deltaTime;
        
        if(life < 0)
        {
            
            Destroy(gameObject);
        }
    }
}
