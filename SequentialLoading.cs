using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class SequentialLoading : MonoBehaviour
{
    public RawImage[] rawImage;

    private Texture2D _texture2D;
    public async void OnSequentialDownloadImage()
    {
        foreach (var item in rawImage)
        {
            _texture2D = await GetRemoteTexture("https://picsum.photos/200/300");
            item.texture = _texture2D;
        }
    }
    private static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
        var asyncOp = webRequest.SendWebRequest();

        while (asyncOp.isDone == false)
        {
            await Task.Delay(1000);
        }
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
                break;
        }
        return DownloadHandlerTexture.GetContent(webRequest);
    }
}
