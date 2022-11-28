using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.TimeZoneInfo;

public class ChangeScene : MonoBehaviour
{
    public float speed;
    public int sceneId;
    public string NumberMap;

    public Animator transitions;

    public float TransitionTime = 1;

    public void Start()
    {
        transitions.SetTrigger("Start");
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        yield return new WaitForSeconds(10);
        transitions.SetTrigger("Load");
        yield return new WaitForSeconds(TransitionTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        yield return new WaitForSeconds(10);
        while (!operation.isDone)
        {
            yield return new WaitForSeconds(10);
            yield return null;
        }
    }
}
