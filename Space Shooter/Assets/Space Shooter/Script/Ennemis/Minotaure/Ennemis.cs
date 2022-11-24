using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
[RequireComponent(typeof(LifeStats))]

public class Ennemis : MonoBehaviour
{
    public LifeStats stats;
    [Space(10)]
    [Header("Int")]
    public int RandomDropShield;

    [Space(10)]
    [Header("GameObject/List")]
    public GameObject explosionPrefabs;
    public GameObject bullet;
    public GameObject ShieldSpaceBall;
    public Transform[] FirePoints;

    [Space(10)]
    [Header("Bools")]
    public bool isAlvie;

    private Asteroid_Field field;
    private GameManager gameManager;
    private Bullet_Controller bulletController;
    private float firepointlist;
    public float fireRate;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        stats.currentHealth = stats.MaxHealth;
        bulletController = bullet.gameObject.GetComponent<Bullet_Controller>();
        field = GameObject.FindObjectOfType<Asteroid_Field>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        firepointlist = FirePoints.Count();
        bullet.GetComponent<BulletEnnemis>().dammage = stats.Damage;
    }

    // Update is called once per frame
    void Update()
    {

        if (stats.currentHealth <= 0)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            //field.asteroidsClones.Remove(gameObject);
            gameManager.money += stats.MoneyDrop;
            RandomDropShield = Random.Range(1, 5);
            Debug.Log(RandomDropShield);
            if (RandomDropShield == 4)
            {
                Instantiate(ShieldSpaceBall, transform.position, transform.rotation);
            }
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
                for (int i = 0; i < firepointlist; i++)
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
        if (collision.gameObject.tag == "DestroyAsteroid" || collision.gameObject.tag == "Player")
        {
            //field.asteroidsClones.Remove(gameObject);
            isAlvie = false;
            Destroy(gameObject);
        }
    }

}
