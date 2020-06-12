using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpace : MonoBehaviour
{
    [SerializeField] private GameObject tower;
    [SerializeField] private float clickRadius = 0.3f;
    private Camera _camera;

    private bool _readyToPlace = false;
    private GameObject _pendingTower;

    // Update is called once per frame
    private void Start()
    {
        _camera = Camera.main;
        GlobalEventHandler.handler.onTowerSelected += OnTowerSelected;
        GlobalEventHandler.handler.onTowerPlaced += OnTowerPlaced;
    }

    private void OnDisable()
    {
        GlobalEventHandler.handler.onTowerSelected -= OnTowerSelected;
        GlobalEventHandler.handler.onTowerPlaced -= OnTowerPlaced;
    }
    private void OnDestroy()
    {
        GlobalEventHandler.handler.onTowerSelected -= OnTowerSelected;
        GlobalEventHandler.handler.onTowerPlaced -= OnTowerPlaced;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _readyToPlace)
        {
            if (_camera != null)
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);

                float distanceFromSpace = Vector2.Distance(mousePos, transform.position);
                if (distanceFromSpace < clickRadius)
                {
                    if (tower != null)
                        Destroy(tower);
                    
                    Debug.Log("Tower Placed At " + transform.position);
                    tower = Instantiate(_pendingTower);
                    tower.transform.position = transform.position;
                    GlobalEventHandler.handler.TowerPlaced();
                }
            }
        }
    }

    void OnTowerSelected()
    {
        print("Ready to place");
        _pendingTower = GlobalEventHandler.selectedTower;
        _readyToPlace = true;
    }

    void OnTowerPlaced()
    {
        print("Acknowledge tower placement");
        _pendingTower = new GameObject();
        _readyToPlace = false;
    }
    
    void OnDrawGizmos()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, clickRadius);
    }
}
