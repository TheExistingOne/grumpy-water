using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShooterTower : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField, Range(0, 5)]  private float fireRate = 0.2f;
    [SerializeField]               private GameObject bullet;

    [Header("Multishot")] 
    [SerializeField]               private bool multishot = false;
    [SerializeField, Range(1, 10)] private int fireCount = 1;
    [SerializeField, Range(0, 1)]  private float multishotDelay = 0.1f;
    [SerializeField, Range(0, 100)] private float spread = 10f;

    private GameObject _target;
    private GameObject[] _enemies;
    
    private bool _shotInProgress = false;
    
    float _minMultiOffset, _multiOffsetStep, _multishotOffset;

    public ShooterTower()
    {
        _multiOffsetStep = spread / fireCount;
        _minMultiOffset = -(spread / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_shotInProgress)
        {
            PrepareShot();
        }

        if (_target != null)
        {
            Vector3 rel = _target.transform.position - transform.position;

            float angle = (Mathf.Atan2(rel.y, rel.x) * 180 / Mathf.PI) - 90;

            transform.rotation =
                Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.eulerAngles.y, angle));
        }
    }

    void PrepareShot()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (!(_enemies.Length < 1))
        {
            _target = _enemies[0];
            
            /*foreach (GameObject enemy in _enemies)
            {
                
                if (_target == null)
                {
                    _target = enemy;
                }
                else
                {
                    float dist = Vector2.Distance(gameObject.transform.position, enemy.transform.position);
                    float curDist = Vector2.Distance(gameObject.transform.position, _target.transform.position);
                    if (curDist > dist)
                    {
                        _target = enemy;
                    }
                }
                
            }
            */
            
            StartCoroutine(Shoot(_target));
            _shotInProgress = true;
        }
    }
    
    IEnumerator Shoot(GameObject target)
    {
        if (multishot)
        {
            Vector3 rotationalOffset = transform.rotation.eulerAngles;
            _multishotOffset = _minMultiOffset;

            for (int i = 0; i < fireCount; i++)
            {
                GameObject shot = Instantiate(bullet);
                shot.transform.position = gameObject.transform.position;
                shot.transform.rotation = gameObject.transform.rotation;
                shot.GetComponent<Projectile>().tracked = _target;
                
                yield return new WaitForSeconds(multishotDelay);
            }
            
            yield return new WaitForSeconds(fireRate);
            _shotInProgress = false;
        }
        else
        {
            GameObject shot = Instantiate(bullet);
            shot.transform.position = gameObject.transform.position;
            shot.transform.rotation = gameObject.transform.rotation;
            shot.GetComponent<Projectile>().tracked = _target;

            yield return new WaitForSeconds(fireRate);
            _shotInProgress = false;
        }
    }
}
