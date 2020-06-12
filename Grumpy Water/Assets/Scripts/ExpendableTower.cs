using System;
using System.Security.Cryptography;
using UnityEngine;

public class ExpendableTower : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float damageCapacity = 5f;

    private float _remainingCapacity;

    void Start()
    {
        _remainingCapacity = damageCapacity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            TowerDefenseEnemy enemy = other.GetComponent<TowerDefenseEnemy>();
            float damage = Mathf.Min(enemy.CurrHealth, _remainingCapacity);
            
            enemy.DamageBy(damage);
            _remainingCapacity -= damage;
            if (_remainingCapacity < 0.01f)
                Destroy(gameObject);
        }
    }
}