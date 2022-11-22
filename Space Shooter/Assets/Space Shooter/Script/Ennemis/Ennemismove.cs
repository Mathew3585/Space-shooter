using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI; //important
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Ennemismove : MonoBehaviour
{
    public float timer; 

    public float timerMax;

    public float titleAngle;
    public float Force;

    public int Speed;

    public int randDir;

    Rigidbody rb;


    public Transform targetLeft;

    public Transform targetRight;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {

        timer += Time.deltaTime;
        if (timer > timerMax)
        {
            // pick a new direction
            randDir = Random.Range(1, 3);
            Debug.Log(randDir);
            timer = 0;
        }
        // movement here
        // ....
        if(randDir == 1)
        {
            transform.position = Vector3.Lerp(transform.position, targetLeft.position, Time.deltaTime);
            rb.rotation = Quaternion.Euler(Vector3.forward * Force * -titleAngle);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        if (randDir == 2)
        {
            transform.position = Vector3.Lerp(transform.position, targetRight.position, Time.deltaTime);
            rb.rotation = Quaternion.Euler(Vector3.forward * Force * titleAngle);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BlockPalyer")
        {
            Debug.Log("collision");
            randDir = Random.Range(1, 3);
        }
    }

}
