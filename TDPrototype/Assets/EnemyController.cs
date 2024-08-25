using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float speed = 5;
    [SerializeField] public float _yVelocity;
    [SerializeField] public float range = 100;
    [SerializeField] public Material flashingmat;
    [SerializeField] public GameObject target;
    [SerializeField] public float _gravity = .6f;
    public float hp = 10;
    private CharacterController _characterController;
    private NavMeshAgent agent;
    public float damage = 1;
    private MeshRenderer meshRenderer;
    private Material normalmat;
    private Coroutine flashCoroutine;

    public GameObject ps;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
        meshRenderer = GetComponent<MeshRenderer>();
        normalmat = meshRenderer.material;
        tag = "Enemy";
    }

    void FixedUpdate()
    {
        if (!target)
        {
            target = findNearestTrain(gameObject);
        }
        var dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist < range)
        {
            agent.SetDestination(target.transform.position);
        }
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
        //print(Physics.Raycast(transform.position, -Vector3.up, 1.5f));
        /*
        if (Physics.Raycast(transform.position, -Vector3.up, 1.5f, 1))
        {
            if (_yVelocity < 0)
            {
                *yVelocity = *gravity;
            }
        }
        else if (_yVelocity > -3f)
        {
            *yVelocity -= *gravity;
        }

        var movevec = new Vector3();
        movevec.y = _yVelocity;
        _characterController.Move(movevec * Time.deltaTime);
        */
        if (hp < 0)
        {
            GameObject.FindFirstObjectByType<EnemySpawnerScript>().addMoney();
            hp = 100000000;
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            Instantiate(ps, transform.position, Quaternion.identity);
            DestroyImmediate(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashMaterial());

            hp -= other.GetComponent<BulletScript>().damage;
            other.GetComponent<BulletScript>().damage *= .5f;
            Destroy(other.gameObject);


        }
    }

    private IEnumerator FlashMaterial()
    {
        meshRenderer.material = flashingmat;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material = normalmat;
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
