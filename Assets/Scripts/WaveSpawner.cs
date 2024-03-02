using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public List<GameObject> enemiesList;

    public GameObject path;

    private Vector3 spawnOrigin;

    private Dictionary<string, GameObject> enemies;

    void Start()
    {
        spawnOrigin = new Vector3(0, 0, 0);
        enemies = enemiesList.ToDictionary(x => x.name, x => x);
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        for(int i = 0; i < 10; i++)
        {
            Instantiate(enemies["Enemy"], spawnOrigin, Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
