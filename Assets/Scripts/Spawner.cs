using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] float spawnDelay = 10.0f;
    float lastSpawn = -Mathf.Infinity;
    void FixedUpdate()
    {
        if (Time.time - lastSpawn < spawnDelay)
            return;
        GameObject.Instantiate(creaturePrefab, transform.position, Quaternion.identity);
        lastSpawn = Time.time;
    }
}
