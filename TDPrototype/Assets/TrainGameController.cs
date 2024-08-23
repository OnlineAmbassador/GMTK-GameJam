using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TrainGameController : MonoBehaviour
{
    [SerializeField] public List<GameObject> Carts = new List<GameObject>();
    [SerializeField] public List<GameObject> PreviewCarts = new List<GameObject>();
    // Start is called before the first frame update
    public bool DecelCarts;
    public List<int> splits = new List<int>();
    [SerializeField] public float friction;
    public float accelCurrent;
    [SerializeField] public float accelMax = 10f;
    [SerializeField] public float accelMin = -10f;
    [SerializeField] public float speedMax = 30f;
    [SerializeField] public float speedMin = -15f;

    [SerializeField] public GameObject cartFab;
    [SerializeField] public GameObject tescartFab;
    [SerializeField] public GameObject previewcartFab;
    [SerializeField] public GameObject tespreviewcartFab;

    void Start()
    {
        stopTrains();


    }

    // Update is called once per frame
    void Update()
    {
        ControlTrain();
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     AddCart();
        // }
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     DestroyCart(1);
        // }

        

    }

    public void DestroyCart(int index)
    {
        if (Carts.Count > 1)
        {
            Destroy(Carts[index], 0);
            Destroy(PreviewCarts[index], 0);
            Carts.RemoveAt(index);
            PreviewCarts.RemoveAt(index);
            int test = 0;
            for (int i = 0; i < Carts.Count; i++)
            {
                {
                    if (test == 0)
                    {
                        test++; continue;
                    }
                    Carts[i].GetComponent<CinemachineDollyCart>().m_Position = Carts[i - 1].GetComponent<CinemachineDollyCart>().m_Position - 6;
                }
            }
            for (int i = 0; i < Carts.Count; i++)
            {
                if (test == 1)
                {
                    test++; continue;
                }
                PreviewCarts[i].GetComponent<CinemachineDollyCart>().m_Position = PreviewCarts[i - 1].GetComponent<CinemachineDollyCart>().m_Position - 6;
            } 
        }
    }

    public void AddCart(int carttype = 0)
    {
        switch (carttype) {
            case 0:
            if (Carts.Count != 0)
            {
                GameObject go = Instantiate(cartFab, Vector3.zero, Quaternion.identity);
                go.GetComponent<CinemachineDollyCart>().m_Position = Carts[Carts.Count - 1].GetComponent<CinemachineDollyCart>().m_Position - 6;
                go.GetComponent<CinemachineDollyCart>().m_Speed = Carts[Carts.Count - 1].GetComponent<CinemachineDollyCart>().m_Speed;
                go.GetComponent<CinemachineDollyCart>().m_Path = Carts[Carts.Count - 1].GetComponent<CinemachineDollyCart>().m_Path;
                Carts.Add(go);

                GameObject po = Instantiate(previewcartFab, Vector3.zero, Quaternion.identity);
                po.GetComponent<CinemachineDollyCart>().m_Position = PreviewCarts[PreviewCarts.Count - 1].GetComponent<CinemachineDollyCart>().m_Position - 6;
                po.GetComponent<CinemachineDollyCart>().m_Speed = 0;
                po.GetComponent<CinemachineDollyCart>().m_Path = PreviewCarts[PreviewCarts.Count - 1].GetComponent<CinemachineDollyCart>().m_Path;
                PreviewCarts.Add(po);
            }
                break;
            case 1:
                if (Carts.Count != 0)
                {
                    GameObject go = Instantiate(tescartFab, Vector3.zero, Quaternion.identity);
                    go.GetComponent<CinemachineDollyCart>().m_Position = Carts[Carts.Count - 1].GetComponent<CinemachineDollyCart>().m_Position - 6;
                    go.GetComponent<CinemachineDollyCart>().m_Speed = Carts[Carts.Count - 1].GetComponent<CinemachineDollyCart>().m_Speed;
                    go.GetComponent<CinemachineDollyCart>().m_Path = Carts[Carts.Count - 1].GetComponent<CinemachineDollyCart>().m_Path;
                    Carts.Add(go);

                    GameObject po = Instantiate(tespreviewcartFab, Vector3.zero, Quaternion.identity);
                    po.GetComponent<CinemachineDollyCart>().m_Position = PreviewCarts[PreviewCarts.Count - 1].GetComponent<CinemachineDollyCart>().m_Position - 6;
                    po.GetComponent<CinemachineDollyCart>().m_Speed = 0;
                    po.GetComponent<CinemachineDollyCart>().m_Path = PreviewCarts[PreviewCarts.Count - 1].GetComponent<CinemachineDollyCart>().m_Path;
                    PreviewCarts.Add(po);
                }
                break;
        }
    }

    void stopTrains()
    {
        for (int i = 0; i < Carts.Count; i++)
        {
            Carts[i].GetComponent<CinemachineDollyCart>().m_Speed = 0;
        }
    }

    void ControlTrain()
    {
        float vinput = Input.GetAxis("Vertical");
        accelCurrent += vinput*3f * Time.deltaTime;
        accelCurrent *= Mathf.Abs(vinput);
        accelCurrent = Mathf.Clamp(accelCurrent, accelMin, accelMax);

        AccelCarts(accelCurrent);
    }

    void AccelCarts(float val)
    {
        for (int i = 0; i < Carts.Count; i++)
        {
            float curSpeed = Carts[i].GetComponent<CinemachineDollyCart>().m_Speed;
            Carts[i].GetComponent<CinemachineDollyCart>().m_Speed = Mathf.Clamp(curSpeed + (val * Time.deltaTime) - friction * curSpeed * Time.deltaTime, speedMin, speedMax);
        }
    }

}