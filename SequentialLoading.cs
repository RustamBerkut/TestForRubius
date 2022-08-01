using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SequentialLoading : MonoBehaviour
{
    public RawImage rawImage;
    private Texture2D _texture2D;
    private void Start()
    {
        StartCoroutine(OnDownloadImage("https://picsum.photos/200/300"));
    }

    IEnumerator OnDownloadImage(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.downloadHandler = new DownloadHandlerTexture();
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                    _texture2D = DownloadHandlerTexture.GetContent(webRequest);
                    rawImage.texture = _texture2D;
                    break;
            }
        }
    }
}
