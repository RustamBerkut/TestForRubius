using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class AsyncLoading : MonoBehaviour
{
    public RawImage[] rawImage;
    public async void OnAsyncDownloadImage()
    {
        var task1 = OnDownloadImage("https://picsum.photos/200/300");
        var task2 = OnDownloadImage("https://picsum.photos/200/300");
        var task3 = OnDownloadImage("https://picsum.photos/200/300");
        var task4 = OnDownloadImage("https://picsum.photos/200/300");

        Texture2D[] texture2d = await Task.WhenAll(task1, task2, task3, task4);

        for (int i = 0; i < rawImage.Length; i++)
        {
            rawImage[i].texture = texture2d[i];
        }
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
}
