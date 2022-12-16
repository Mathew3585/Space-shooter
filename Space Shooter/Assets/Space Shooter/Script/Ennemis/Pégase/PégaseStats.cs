using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LifeStats))]
public class PégaseStats : MonoBehaviour
{
    public LifeStats stats;

    [Space(10)]
    public float timer;

    public float timerMax;

    [Space(10)]
    public float titleAngle;
    public float Force;

    [Space(10)]
    public float XAxiesMin;
    public float XAxiesMax;

    [Space(10)]
    public int Speed;

    public int Power;

    public int randDir;
    public int RandomDropShield;

    public Transform targetLeft;

    public Transform targetRight;

    public Transform rootObject;
    private GameObject rootGameObject;
    public GameObject explosionPrefabs;
    public GameObject LifeSpaceBall;
    public GameObject ShieldSpaceBall;

    public bool Left;
    public bool Right;

    private Ship_Controller ship_Controller;
    private Asteroid_Field field;
    private GameManager gameManager;
    private CameraShaker cameraShaker;
    bool IsAlive;


    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        cameraShaker = GameObject.FindObjectOfType<CameraShaker>();
        ship_Controller = FindObjectOfType<Ship_Controller>();
        field = GameObject.FindObjectOfType<Asteroid_Field>();
    }
    // Start is called before the first frame update
    void Start()
    {
        IsAlive = true;
        stats.currentHealth = stats.MaxHealth;
        rootGameObject = rootObject.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 Position = rootObject.position;
        Position.z += Time.deltaTime * Speed;
        rootObject.position = Position;

        timer += Time.deltaTime;


        if (stats.currentHealth <= 0)
        {
            IsAlive = false;
        }

            if (timer > timerMax)
        {
            // pick a new direction
            randDir = Random.Range(1, 3);
            Debug.Log(randDir);
            timer = 0;
        }
        // movement here
        // ....
        if (randDir == 1)
        {
            Right = false;
            Left = true;
            if (rootObject.position.x <= XAxiesMin && Left)
            {
                randDir = 2;
                Debug.Log("Limit left");
            }
            rootObject.position = Vector3.Lerp(rootObject.position, targetLeft.position, Time.deltaTime * Speed);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Force * -titleAngle);
            rootObject.rotation = Quaternion.Lerp(rootObject.rotation, rotation, Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, 0.8f, transform.position.z);

        }

        else if (randDir == 2)
        {
            Right = true;
            Left = false;
            if (rootObject.position.x >= XAxiesMax && Right)
            {
                randDir = 1;
                Debug.Log("Limit Right");
            }
            rootObject.position = Vector3.Lerp(rootObject.position, targetRight.position, Time.deltaTime * Speed);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Force * titleAngle);
            rootObject.rotation = Quaternion.Lerp(rootObject.rotation, rotation, Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, 0.8f, transform.position.z);

        }

        if (IsAlive == false)
        {
            field.PégasNumber--;
            field.asteroidsClones.Remove(gameObject);
            Destroy(rootGameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            gameManager.money += stats.MoneyDrop;
            IsAlive = false;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            ship_Controller.shipStats.CurrentHealth -= stats.Damage;
            cameraShaker.ShakeOnce(3f, 3f, 0.5f, 0.5f);
            IsAlive = false;
        }

        if (collision.gameObject.CompareTag("Sheild"))
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<ShieldDommageDetect>().ship_Controller.shipStats.CurrentShield -= stats.Damage;
            IsAlive = false;
        }

        if (collision.gameObject.tag == "DestroyAsteroid")
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            IsAlive = false;
        }


        if (collision.gameObject.CompareTag("Ultimate"))
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity);
            gameManager.money += stats.MoneyDrop;
            if (ship_Controller.shipStats.CurrentPower == ship_Controller.shipStats.maxPower)
            {
                gameManager.game.ship_Controller.shipStats.CurrentPower += 0;
            }
            else
            {
                gameManager.game.ship_Controller.shipStats.CurrentPower += Power;
            }
            IsAlive = false;
        }
    }
}
