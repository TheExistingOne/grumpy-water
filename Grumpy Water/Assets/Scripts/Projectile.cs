using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0, 40)] private float baseDamage = 1f;
    [SerializeField, Range(0, 20)]  private float speed = 1f;
    [SerializeField, Range(0, 5)]  private float lifetime = 1f;
    [SerializeField, Range(1, 10)] private int peirce = 1;
    [SerializeField, Range(0, 1)] private float trackingHarshness = 0.1f;
    [SerializeField, Range(0, 1)] private float trackingDelay = 0.05f;
    
    private int _peirced = 0;
    public GameObject tracked;
    private bool _readyToTrack = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifetime(lifetime));
        StartCoroutine(TrackingDelay(trackingDelay));
    }

    // Update is called once per frame
    void Update() 
    {
        if (_readyToTrack && tracked != null)
        {
            Vector3 rel = tracked.transform.position - transform.position;

            float angle = (Mathf.Atan2(rel.y, rel.x) * 180 / Mathf.PI) - 90;

            transform.rotation = Quaternion.Lerp(transform.rotation,
                quaternion.Euler(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle)),
                trackingHarshness);
        }

        transform.position += transform.up * speed * Time.deltaTime;
    }

    IEnumerator Lifetime(float countdown)
    {
        yield return new WaitForSeconds(countdown);
        
        Destroy(gameObject);
    }
    IEnumerator TrackingDelay(float countdown)
    {
        yield return new WaitForSeconds(countdown);

        _readyToTrack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<TowerDefenseEnemy>().DamageBy(baseDamage);
            _peirced++;
        }

        if (_peirced >= peirce)
        {
            Destroy(gameObject);
        }
    }
}
