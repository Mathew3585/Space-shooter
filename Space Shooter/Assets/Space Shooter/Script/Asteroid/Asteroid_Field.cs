using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Asteroid_Field : MonoBehaviour
{
    [Header("GameManger")]
    public GameManager gameManager;

    [Space(10)]

    public GameObject[] asteroid;
    public int NumberOfEnnemis;

    [HideInInspector]
    public int[] randomAsteroid;
    [HideInInspector]
    public float[] speedRange;

    public Vector3 spawnRange;

    public int seed;

    public List<GameObject> asteroidsClones ;

    private void Start()
    {
        randomAsteroid = new int[NumberOfEnnemis];
        speedRange = new float[NumberOfEnnemis];
        asteroidsClones = new List<GameObject>(NumberOfEnnemis);
        NumberOfEnnemis = gameManager.NumbresEnnemisPhase1;


        Random.InitState(seed);
        
        if(gameManager.game.Progress >= gameManager.ProgressPhase1)
        {
            for (int i = 0; i < NumberOfEnnemis; i++)
            {

                GameObject Asteroid = Instantiate(asteroid[0], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                          transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                          transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);

                asteroidsClones.Add(Asteroid);
            }
        }

    }


    
    private void Update()
    {

        if (asteroidsClones.Count < NumberOfEnnemis)
        {

            if (gameManager.game.Progress <= gameManager.ProgressPhase1 && gameManager.ValidatePhase1)
            {
                Debug.Log("Phase 1");
                NumberOfEnnemis = gameManager.NumbresEnnemisPhase1;
                for (int i = 0; i < NumberOfEnnemis; i++)
                {

                    GameObject Asteroid = Instantiate(asteroid[0], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                              transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                              transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                    asteroidsClones.Add(Asteroid);

                }
            }


            else if (gameManager.game.Progress >= gameManager.ProgressPhase2 && gameManager.ValidatePhase2)
            {
                NumberOfEnnemis = gameManager.NumbresEnnemisPhase2;
                for (int i = 0; i < NumberOfEnnemis; i++)
                {
                    randomAsteroid[i] = Random.Range(0, 2);

                    GameObject Asteroid = Instantiate(asteroid[randomAsteroid[i]], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                              transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                              transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                    Debug.Log("Phase ");
                    asteroidsClones.Add(Asteroid);
                }
            }

            else if (gameManager.game.Progress >= gameManager.ProgressPhase3 && gameManager.ValidatePhase3)
            {
                NumberOfEnnemis = gameManager.NumbresEnnemisPhase3;
                for (int i = 0; i < NumberOfEnnemis; i++)
                {
                    GameObject Asteroid = Instantiate(asteroid[Random.Range(0, 4)], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                              transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                              transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                    asteroidsClones.Add(Asteroid);
                    Debug.Log("Phase 3");

                }
            }
        }
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnRange) ;
    }
}
