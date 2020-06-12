using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class GlobalEventHandler : MonoBehaviour
{
    public static GlobalEventHandler handler;

    public static GameObject selectedTower;

    public static int globalWave = 0;
    public static float waveDuration = 10f;

    public static int lastDamage;

    public void Awake()
    {
        handler = this;
    }

    public void Start()
    {
    }

    public event Action damageTaken;
    public void DamageTaken() {damageTaken?.Invoke();}

    public event Action onTowerSelected;
    public void TowerSeleceted() {onTowerSelected?.Invoke();}
    
    public event Action onTowerPlaced;
    public void TowerPlaced() {onTowerPlaced?.Invoke();}
}
