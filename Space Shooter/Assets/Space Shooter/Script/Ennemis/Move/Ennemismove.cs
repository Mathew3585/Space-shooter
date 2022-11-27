using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI; //important
using UnityEngine.Rendering;
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

    public Transform targetLeft;

    public Transform targetRight;

    public Transform rootObject;

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
            rootObject.position = Vector3.Lerp(rootObject.position, targetLeft.position, Time.deltaTime);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Force * -titleAngle);
            rootObject.rotation = Quaternion.Lerp(rootObject.rotation, rotation, Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, 0.8f, transform.position.z);
        }
        if (randDir == 2)
        {
            rootObject.position = Vector3.Lerp(rootObject.position, targetRight.position, Time.deltaTime);
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Force * titleAngle);
            rootObject.rotation = Quaternion.Lerp(rootObject.rotation, rotation, Time.deltaTime);
            rootObject.position = new Vector3(rootObject.position.x, 0.8f, transform.position.z);
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.tag == "BlockPalyer")
        {
            Debug.Log("collision");
            randDir = Random.Range(1, 3);
        }
    }
}
