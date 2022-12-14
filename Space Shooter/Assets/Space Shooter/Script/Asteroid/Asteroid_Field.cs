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

    public List<GameObject> asteroidsClones;

    public int PégasNumberMax;
    public int HydreNumberMax;
    public int AsteroidNumberMax;

    [HideInInspector]
    public int PégasNumber;
    [HideInInspector]
    public int AsteroidNumber;

    [HideInInspector]
    public int HydreNumber;

    public bool game;

    private void Start()
    {
        randomAsteroid = new int[NumberOfEnnemis];
        speedRange = new float[NumberOfEnnemis];
        asteroidsClones = new List<GameObject>(NumberOfEnnemis);
        NumberOfEnnemis = gameManager.NumbresEnnemisPhase1;
        AsteroidNumber = 0;


        Random.InitState(seed);

        if (game)
        {
            if (gameManager.game.Progress >= gameManager.ProgressPhase1)
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


    }



    private void Update()
    {
        if (game == false)
        {
            NumberOfEnnemis = AsteroidNumberMax;

            for (int i = 0; i < NumberOfEnnemis; i++)
            {
                Debug.Log(i);
                if (AsteroidNumber < AsteroidNumberMax)
                {
                    GameObject Asteroid = Instantiate(asteroid[Random.Range(0,4)], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                          transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                          transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                    asteroidsClones.Add(Asteroid);

                    if (Asteroid.CompareTag("Asteroid"))
                    {
                        AsteroidNumber++;
                    }

                }
            }
        }

        else if (asteroidsClones.Count < NumberOfEnnemis)
        {
            if (game)
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
                        if (PégasNumber < PégasNumberMax)
                        {
                            GameObject Asteroid = Instantiate(asteroid[Random.Range(0, 2)], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                  transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                  transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                            if (Asteroid.CompareTag("Pégas"))
                            {
                                PégasNumber++;
                            }
                            asteroidsClones.Add(Asteroid);
                        }
                        if (PégasNumber == PégasNumberMax)
                        {
                            GameObject Minuator = Instantiate(asteroid[0], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                  transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                  transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                            asteroidsClones.Add(Minuator);
                        }

                        Debug.Log("Phase 2");

                    }
                }

                else if (gameManager.game.Progress >= gameManager.ProgressPhase3 && gameManager.ValidatePhase3)
                {
                    NumberOfEnnemis = gameManager.NumbresEnnemisPhase3;

                    for (int i = 0; i < NumberOfEnnemis; i++)
                    {
                        if (PégasNumber < PégasNumberMax && HydreNumber < HydreNumberMax)
                        {
                            GameObject Ennemis = Instantiate(asteroid[Random.Range(0, 3)], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                  transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                  transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                            if (Ennemis.CompareTag("Pégas"))
                            {
                                PégasNumber++;
                            }

                            if (Ennemis.CompareTag("Hydre"))
                            {
                                HydreNumber++;
                            }
                            asteroidsClones.Add(Ennemis);
                        }

                        if (PégasNumber == PégasNumberMax)
                        {
                            GameObject Minuator = Instantiate(asteroid[0], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                  transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                  transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                            asteroidsClones.Add(Minuator);
                        }

                        if (HydreNumber == HydreNumberMax)
                        {
                            GameObject Minuator = Instantiate(asteroid[0], new Vector3(transform.position.x + Random.Range(-spawnRange.x, spawnRange.x),
                                                                                  transform.position.y + Random.Range(-spawnRange.y, spawnRange.y),
                                                                                  transform.position.z + Random.Range(-spawnRange.z, spawnRange.z)), Quaternion.identity);
                            asteroidsClones.Add(Minuator);
                        }
                    }
                    Debug.Log("Phase 3");
                }

                else if (gameManager.game.Progress >= gameManager.BossFight && gameManager.ValidateBossFight)
                {
                    NumberOfEnnemis = gameManager.NumbresBoss;

                    for (int i = 0; i < NumberOfEnnemis; i++)
                    {
                        if (NumberOfEnnemis <= 1)
                        {
                            GameObject Boss = Instantiate(asteroid[3], transform.position, transform.rotation);
                            asteroidsClones.Add(Boss);
                        }

                    }
                }

            }

        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, spawnRange);
    }
}