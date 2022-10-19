using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFx : MonoBehaviour
{

    public Material[] materials;
    public float waitTime;
    GameManager gameManager;
    Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        StartCoroutine(FxToMat());
    }

    IEnumerator FxToMat()
    {
        renderer.material = materials[0];
        yield return new WaitForSeconds(waitTime);
        renderer.material = materials[1];
    }
}
