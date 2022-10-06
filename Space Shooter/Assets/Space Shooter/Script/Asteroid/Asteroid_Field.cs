using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid_Field : MonoBehaviour
{


    public GameObject[] asteroid = new GameObject[3];
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

    private GameObject[] asteroidsClones;

    private void Start()
    {
        randomAsteroid = new int[numberofAsteroid];
        speedRange = new float[numberofAsteroid];
        asteroidsClones = new GameObject[numberofAsteroid];

        
        Random.InitState(seed);

        for (int i = 0; i < numberofAsteroid; i++)
        {
            randomAsteroid[i] = Random.Range(0, 3);
            speedRange[i] = Random.Range(minSpeed, maxSpeed);

            asteroidsClones[i] = Instantiate(asteroid[randomAsteroid[i]], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                      transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                      transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);


            asteroidsClones[i].transform.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speedRange[i];
            asteroidsClones[i].transform.parent = this.transform;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnRange) ;
    }
}
