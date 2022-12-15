using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
[RequireComponent(typeof(LifeStats))]

public class Hydre : MonoBehaviour
{
    public LifeStats stats;
    public GameObject LifeSpaceBall;

    [Space(10)]
    [Header("CrashDamage")]
    public int CrashDamage;

    [Space(10)]
    [Header("Int")]
    public int RandomDropShield;
    public int Power;

    [Space(10)]
    [Header("GameObject/List")]
    public GameObject explosionPrefabs;
    public GameObject bullet;
    public GameObject ShieldSpaceBall;
    public GameObject Minieneis;
    public Transform[] FirePoints;
    public Transform[] SpawnPoint;
    public Transform rootObject;

    [Space(10)]
    [Header("Bools")]
    public bool isAlvie;
    public bool Ennemis;

    private Asteroid_Field field;
    private GameManager gameManager;
    private Bullet_Controller bulletController;
    private Ship_Controller shipController;
    private float firepointlist;
    public float fireRate;
    private float nextFire;
    private CameraShaker cameraShaker;


    private void Awake()
    {
        shipController = FindObjectOfType<Ship_Controller>();
    }
    // Start is called before the first frame update
    void Start()
    {
        stats.currentHealth = stats.MaxHealth;
        bulletController = bullet.gameObject.GetComponent<Bullet_Controller>();
        field = GameObject.FindObjectOfType<Asteroid_Field>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        firepointlist = FirePoints.Count();
        bullet.GetComponent<BulletEnnemis>().dammage = stats.Damage;
        cameraShaker = GameObject.FindObjectOfType<CameraShaker>();
        Ennemis = true;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 Position = rootObject.position;
        Position.z += Time.deltaTime * stats.Speed;
        rootObject.position = Position;

        if (stats.currentHealth <= 0)
        {
            isAlvie = false;
        }

        if (stats.currentHealth <= 30 && Ennemis)
        {
            Ennemis = false;
            for (int i = 0; i < SpawnPoint.Length; i++)
            {
                Instantiate(Minieneis, SpawnPoint[i].position, Quaternion.identity);
            }
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

        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (isAlvie == false)
            {
                Dead();
            }
        }

        if (collision.gameObject.tag == "Sheild")
        {
            isAlvie = false;
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            field.asteroidsClones.Remove(gameObject);
            field.HydreNumber--;
            if (shipController.shipStats.CurrentPower == shipController.shipStats.maxPower)
            {
                gameManager.game.ship_Controller.shipStats.CurrentPower += 0;
            }
            else
            {
                gameManager.game.ship_Controller.shipStats.CurrentPower = +Power;
            }
            RandomDropShield = Random.Range(1, 11);
            Debug.Log(RandomDropShield);
            if (RandomDropShield == 9)
            {
                Instantiate(ShieldSpaceBall, transform.position, transform.rotation);
            }
            else if (RandomDropShield == 6)
            {
                Instantiate(LifeSpaceBall, transform.position, transform.rotation);
            }
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
                cameraShaker.ShakeOnce(3f, 3f, 0.5f, 0.5f);
                field.HydreNumber--;
                if (shipController.shipStats.CurrentPower == shipController.shipStats.maxPower)
                {
                    gameManager.game.ship_Controller.shipStats.CurrentPower += 0;
                }
                else
                {
                    gameManager.game.ship_Controller.shipStats.CurrentPower = +Power;
                }
                RandomDropShield = Random.Range(1, 11);
                Debug.Log(RandomDropShield);
                if (RandomDropShield == 9)
                {
                    Instantiate(ShieldSpaceBall, transform.position, transform.rotation);
                }
                else if (RandomDropShield == 6)
                {
                    Instantiate(LifeSpaceBall, transform.position, transform.rotation);
                }

                gameManager.money += stats.MoneyDrop;
                collision.transform.GetComponent<Ship_Controller>().shipStats.CurrentHealth -= CrashDamage;
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.tag == "DestroyAsteroid")
        {
            isAlvie = false;
            field.asteroidsClones.Remove(gameObject);
            field.HydreNumber--;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ultimate"))
        {
            Dead();
            Debug.Log("Enter ulti");
        }



    }
   public void Dead()
    {
        Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
        field.asteroidsClones.Remove(gameObject);
        gameManager.money += stats.MoneyDrop;
        if (shipController.shipStats.CurrentPower == shipController.shipStats.maxPower)
        {
            gameManager.game.ship_Controller.shipStats.CurrentPower += 0;
        }
        else
        {
            gameManager.game.ship_Controller.shipStats.CurrentPower = +Power;
        }
        RandomDropShield = Random.Range(1, 11);
        Debug.Log(RandomDropShield);
        if (RandomDropShield == 9)
        {
            Instantiate(ShieldSpaceBall, transform.position, transform.rotation);
        }
        else if (RandomDropShield == 6)
        {
            Instantiate(LifeSpaceBall, transform.position, transform.rotation);
        }
        Debug.Log(RandomDropShield);

        Destroy(gameObject);
    }
}
