using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaggeredSpawner : MonoBehaviour
{
    [SerializeField] private GameObject toSpawn;
    [SerializeField] private int wave;
    [SerializeField, Range(1, 10)] private int wavesPerSpawn = 2;
    [SerializeField, Range(1, 10)] private int spawnsPerWave = 1;
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

        bool spawnNow = ((wave % wavesPerSpawn) == 0);

        if (wave > 0 && spawnNow){

            for (int i = 0; i < spawnsPerWave; i++)
            {
                GameObject spawned = Instantiate(toSpawn);
                spawned.transform.position = transform.position;

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }
        yield return new WaitForSeconds(3);

        wave += 1;
        _waveSpawned = false;
    }
}