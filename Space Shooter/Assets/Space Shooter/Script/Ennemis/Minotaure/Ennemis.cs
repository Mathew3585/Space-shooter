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
    [Header("CrashDamage")]
    public int CrashDamage;

    [Space(10)]
    [Header("Int")]
    public int RandomDropShield;

    [Space(10)]
    [Header("GameObject/List")]
    public GameObject explosionPrefabs;
    public GameObject bullet;
    public GameObject ShieldSpaceBall;
    public Transform[] FirePoints;
    public Transform rootObject;

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

        Vector3 Position = rootObject.position;
        Position.z += Time.deltaTime * stats.Speed;
        rootObject.position = Position;

        if (stats.currentHealth <= 0)
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            field.asteroidsClones.Remove(gameObject);
            gameManager.money += stats.MoneyDrop;
            RandomDropShield = Random.Range(1, 11);
            Debug.Log(RandomDropShield);
            if (RandomDropShield == 9)
            {
                Instantiate(ShieldSpaceBall, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }

        Collider[] shipCollider = transform.GetComponentsInChildren<Collider>();
        if (isAlvie)
        {
            nextFire -= Time.deltaTime;
            if (nextFire <= 0)
            {
                for (int i = 0; i < firepointlist; i++)
                {
                    GameObject bulletClone = Instantiate(bullet, FirePoints[i].position, bullet.transform.rotation);

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

        if (collision.gameObject.tag == "Sheild")
        {
            isAlvie = false;
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            field.asteroidsClones.Remove(gameObject);
            gameManager.money += stats.MoneyDrop;
            collision.gameObject.GetComponent<ShieldDommageDetect>().ship_Controller.shipStats.CurrentShield -= CrashDamage;
            Destroy(gameObject);
        }

        else if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.GetComponent<Ship_Controller>().ShieldActivate == false)
            {
                isAlvie = false;
                Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
                field.asteroidsClones.Remove(gameObject);
                gameManager.money += stats.MoneyDrop;
                collision.transform.GetComponent<Ship_Controller>().shipStats.CurrentHealth -= CrashDamage;
                Destroy(gameObject);
            }
        }



        if (collision.gameObject.tag == "DestroyAsteroid")
        {
            isAlvie = false;
            field.asteroidsClones.Remove(gameObject);
            gameManager.money += stats.MoneyDrop;
            Destroy(gameObject);
        }
    }

}
