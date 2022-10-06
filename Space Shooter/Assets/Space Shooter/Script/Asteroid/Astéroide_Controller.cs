using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Asteroid_Stats 
{
    public float MaxHealth;
    public float currentHealth;
}

public class Astéroide_Controller : MonoBehaviour
{
    public Asteroid_Stats stats;
    private Quaternion randomRotaion;
    

    // Start is called before the first frame update
    void Start()
    {
        stats.currentHealth = stats.MaxHealth;

        randomRotaion = Random.rotation;
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
}
