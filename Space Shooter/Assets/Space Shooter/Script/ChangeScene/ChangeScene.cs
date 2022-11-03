using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public Image LoadingBarFill;
    public float speed;
    public int sceneId;


    public void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(10);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        yield return new WaitForSeconds(10);
        while (!operation.isDone)
        {
            yield return new WaitForSeconds(10);
            yield return null;
        }
    }
}
