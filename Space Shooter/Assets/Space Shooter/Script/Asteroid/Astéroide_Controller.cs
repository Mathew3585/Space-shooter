using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class Asteroid_Stats 
{
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    public float dammage;

}

public class Astéroide_Controller : MonoBehaviour
{
    public Asteroid_Stats stats;
    [Header("Argent drop")]
    public int MoneyDrop;
    private Quaternion randomRotaion;
    private Asteroid_Field field;
    private GameManager gameManager;

    public GameObject explosionPrefabs;
    public Ship_Controller shipController;

    // Start is called before the first frame update
    void Start()
    {
        stats.currentHealth = stats.MaxHealth;

        randomRotaion = Random.rotation;
        field = GameObject.FindObjectOfType<Asteroid_Field>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        shipController = GameObject.FindObjectOfType<Ship_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(randomRotaion.eulerAngles * 0.1f * Time.deltaTime);

        if(stats.currentHealth <= 0)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            field.asteroidsClones.Remove(gameObject);
            gameManager.money += MoneyDrop;
            if(shipController.shipStats.CurrentPower == shipController.shipStats.maxPower)
            {
                gameManager.game.ship_Controller.shipStats.CurrentPower += 0;
            }
            else
            {
                gameManager.game.ship_Controller.shipStats.CurrentPower ++;
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "DestroyAsteroid")
        {
            field.asteroidsClones.Remove(gameObject);
            Destroy(gameObject);

        }
    }

}
