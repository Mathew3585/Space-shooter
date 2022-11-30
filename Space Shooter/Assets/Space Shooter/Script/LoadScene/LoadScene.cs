using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public float dealy;
    private float TimeDelay;


    // Update is called once per frame
    void Update()
    {
        TimeDelay += Time.deltaTime;
        if(TimeDelay >= dealy)
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator LoadLevel(int LevelIndex)
    {

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(LevelIndex);
    }
}
