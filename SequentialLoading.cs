using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class SequentialLoading : MonoBehaviour
{
    public RawImage[] rawImage;
    public async void OnSequentialDownloadImage()
    {
        foreach (var item in rawImage)
        {
            Texture2D _texture2D = await OnDownloadImage("https://picsum.photos/200/300");
            item.texture = _texture2D;
        }
        ShowImage();
    }
    private static async Task<Texture2D> OnDownloadImage(string url)
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
    private void ShowImage()
    {
        foreach (var item in rawImage)
        {
            item.CrossFadeColor(Color.blue, 2f, false, true);
        }
    }
}
