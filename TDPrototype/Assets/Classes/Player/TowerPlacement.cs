using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask placementCollideMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask placementCheckMask;
    private GameObject currentPlacingTower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPlacingTower != null)
        {
            Ray camray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(camray, out hitInfo, 100f, placementCollideMask))
            {
                currentPlacingTower.transform.position = hitInfo.point;
            }

            // Cancel Tower Placement
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Destroy(currentPlacingTower);
                currentPlacingTower = null;
                return;
            }

            if(Input.GetMouseButtonDown(0) && hitInfo.collider.gameObject != null)
            {
                if(!hitInfo.collider.gameObject.CompareTag("cantPlace"))
                {
                    BoxCollider towerCollider = currentPlacingTower.gameObject.GetComponent<BoxCollider>();
                    towerCollider.isTrigger = true;

                    Vector3 boxCenter = currentPlacingTower.gameObject.transform.position + towerCollider.center;
                    Vector3 halfExtents = towerCollider.size / 2;
                    
                    if(!Physics.CheckBox(boxCenter, halfExtents, Quaternion.identity, placementCheckMask, QueryTriggerInteraction.Ignore))
                    {

                        GameLoopManager.towersInGame.Add(currentPlacingTower.GetComponent<TowerBehavior>());

                        currentPlacingTower = null;
                        towerCollider.isTrigger = false;
                    }    
                }
            }
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        currentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }
}
