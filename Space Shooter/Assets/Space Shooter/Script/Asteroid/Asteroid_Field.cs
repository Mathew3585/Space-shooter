using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid_Field : MonoBehaviour
{


    public GameObject[] asteroid;
    public int numberofAsteroid;

    [HideInInspector]
    public int[] randomAsteroid;
    [HideInInspector]
    public float[] speedRange;

    public Vector3 spawnRange;
    [Header("Speed Asteroid")]
    public float minSpeed;
    public float maxSpeed;

    public int seed;

    public List<GameObject> asteroidsClones ;

    private void Start()
    {
        randomAsteroid = new int[numberofAsteroid];
        speedRange = new float[numberofAsteroid];
        asteroidsClones = new List<GameObject>(numberofAsteroid);

        
        Random.InitState(seed);

        for (int i = 0; i < numberofAsteroid; i++)
        {
            randomAsteroid[i] = Random.Range(0, asteroid.Length);
            speedRange[i] = Random.Range(minSpeed, maxSpeed);

            GameObject Asteroid = Instantiate(asteroid[randomAsteroid[i]], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                      transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                      transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
            asteroidsClones.Add(Asteroid);
        }
    }


    
    private void Update()
    {
        if (asteroidsClones.Count < numberofAsteroid)
        {
            
            GameObject Asteroid = Instantiate(asteroid[Random.Range(0, asteroid.Length)], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                      transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                      transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
            asteroidsClones.Add(Asteroid);
            float val = speedRange[asteroidsClones.Count - 1] = Random.Range(minSpeed, maxSpeed);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnRange) ;
    }
}
