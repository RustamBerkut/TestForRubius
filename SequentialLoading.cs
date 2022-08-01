using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class SequentialLoading : MonoBehaviour
{
    public RawImage[] rawImage;
    private Texture2D _texture2D;

    public void OnSequentialLoading()
    {
        StartCoroutine(OnDownloadImage("https://picsum.photos/200/300"));
    }
    private IEnumerator OnDownloadImage(string uri)
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

                    foreach (var item in rawImage)
                    {
                        _texture2D = DownloadHandlerTexture.GetContent(webRequest);
                        item.texture = _texture2D;
                    }
                    
                    break;
            }
        }
    }
}
