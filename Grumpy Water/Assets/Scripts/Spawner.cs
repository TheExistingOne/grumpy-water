using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject toSpawn;
    [SerializeField] private int wave;
    [SerializeField, Range(0f, 1f)] private float rateModifier = 0.005f;
    [SerializeField] private float timeBetweenSpawns = 0.5f;

    private bool _waveSpawned = false;
    private int _lastSpawnCount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_waveSpawned)
        {
            StartCoroutine(SpawnEnimies());
            _waveSpawned = true;
        }
    }

    IEnumerator SpawnEnimies()
    {
        //int spawnCount = Mathf.CeilToInt(Mathf.Pow(wave * rateModifier, 2));

        int spawnCount = Mathf.CeilToInt((Mathf.Pow(wave,2) * rateModifier) + Mathf.Sqrt(rateModifier * wave));

        if (spawnCount > 0){
            Debug.Log(spawnCount);

            for (int i = 0; i < spawnCount; i++)
            {
                GameObject spawned = Instantiate(toSpawn);
                spawned.transform.position = transform.position;

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
        yield return new WaitForSeconds(3);

        wave += 1;
        _lastSpawnCount = spawnCount;
        _waveSpawned = false;
    }
}
