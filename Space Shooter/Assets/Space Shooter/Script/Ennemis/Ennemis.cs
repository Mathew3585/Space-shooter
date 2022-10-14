using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Ennemis_Stats
{
    public float MaxHealth;
    [HideInInspector]
    public float currentHealth;
    public float dammage;

}

public class Ennemis : MonoBehaviour
{
    public Asteroid_Stats stats;
    [Header("Argent drop")]
    public int MoneyDrop;
    private Asteroid_Field field;
    private GameManager gameManager;
    private Bullet_Controller bulletController;

    public GameObject explosionPrefabs;

    public float fireRate;
    private float nextFire;
    public GameObject bullet;
    public Transform[] FirePoints = new Transform[2];
    public bool isAlvie;
    // Start is called before the first frame update
    void Start()
    {
        stats.currentHealth = stats.MaxHealth;
        bulletController = bullet.gameObject.GetComponent<Bullet_Controller>();
        bulletController.ennemis = true;
        field = GameObject.FindObjectOfType<Asteroid_Field>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (stats.currentHealth <= 0)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            field.asteroidsClones.Remove(gameObject);
            gameManager.money += MoneyDrop;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            nextFire -= Time.fixedDeltaTime;
            if (nextFire <= 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject bulletClone = Instantiate(bullet, FirePoints[i].position, FirePoints[i].rotation);

                    for (int x = 0; x < shipCollider.Length; x++)
                    {
                        Physics.IgnoreCollision(bulletClone.transform.GetComponent<Collider>(), shipCollider[x]);
                    }
                }
                nextFire += 1 / fireRate;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bu")
        {
            field.asteroidsClones.Remove(gameObject);
            isAlvie = false;
            Destroy(gameObject);

        }
    }

}
