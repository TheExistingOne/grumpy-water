using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseEnemy : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)]  private float speed = 3f;
    [SerializeField, Range(1f, 100f)] private float health = 10f;
    [SerializeField]                  private int damage = 1;
    
    [SerializeField, MinMaxSlider(0f, 2f)] private Vector2 scaleLimits;
    [SerializeField] private Vector2[] path = {new Vector2(0,0)};

    [SerializeField] private GameObject renderer;
    
    private int _trackedMarker = 0;
    private bool _stopped = false;
    
    private float _currHealth;

    private ParticleSystem _particleSystem;

    public float CurrHealth => _currHealth;
    
    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _currHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stopped) 
        {
            transform.position = Vector2.MoveTowards(transform.position, path[_trackedMarker], Time.deltaTime * speed);
            if (Vector2.Distance(transform.position, path[_trackedMarker]) < 0.01 && _trackedMarker != path.Length - 1)
            {
                _trackedMarker += 1;
            }

            if (_currHealth <= 0.1f)
            {
                StartCoroutine(OnDeath());
            }

            if (Vector2.Distance(transform.position, path[path.Length - 1]) < 0.05)
            {
                StartCoroutine(OnAttack());
            }
        }
    }

    void LateUpdate()
    {
        if (!_stopped)
        {
            float scale = map(_currHealth, 0f, health, scaleLimits.x, scaleLimits.y);
            renderer.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    IEnumerator OnDeath()
    {
        gameObject.tag = "Untagged";
        _stopped = true;
        renderer.GetComponent<Renderer>().enabled = false;
        _particleSystem.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    
    IEnumerator OnAttack()
    {
        GlobalEventHandler.lastDamage = damage;
        GlobalEventHandler.handler.DamageTaken();
        
        gameObject.tag = "Untagged";
        _stopped = true;
        renderer.GetComponent<Renderer>().enabled = false;
        _particleSystem.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        // Draws a blue line from this transform to the target
        Gizmos.color = Color.green;
        for (int i = 0; i < path.Length; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawLine(transform.position, path[i]);
            }
            else
            {
                Gizmos.DrawLine(path[i], path[i-1]);
            }
        }
    }

    public void DamageBy(float damage)
    {
        _currHealth -= damage;
    }
    
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }
}
