using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
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
    private Quaternion randomRotaion;
    public Asteroid_Field field;



    // Start is called before the first frame update
    void Start()
    {
        stats.currentHealth = stats.MaxHealth;

        randomRotaion = Random.rotation;
        field = GameObject.FindObjectOfType<Asteroid_Field>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(randomRotaion.eulerAngles * 0.1f * Time.deltaTime);

        if(stats.currentHealth <= 0)
        {
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
