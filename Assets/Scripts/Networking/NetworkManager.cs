using System.Collections;
using UnityEngine.Networking;

public class NetworkManager
{
    public IEnumerator SendJsonRequest(string url, string jsonData, System.Action<string> onSuccess, System.Action<string> onError)
    {
        using (UnityWebRequest  www = new UnityWebRequest(url, "POST"))
        {
            
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(www.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(www.error);
            }
        }
    }
    
    public IEnumerator GetRequest(string url, System.Action<string> onSuccess, System.Action<string> onError)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(www.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(www.error);
            }
        }
    }
}