using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

public class BundleAdsLoader : MonoBehaviour
{
    public string pathJson;

    public Action<Sprite> onPreviewLoaded;
    public Action<string> onUrlLoaded;
    public Action<List<Sprite>> onSpritesLoaded;

    public List<Sprite> Sprites;

    private IEnumerator Start()
    {
        DontDestroyOnLoad(gameObject);

        var jsonRequest = UnityWebRequest.Get(pathJson);

        var jsonAsync = jsonRequest.SendWebRequest();

        yield return jsonAsync;

        var jsonText = jsonRequest.downloadHandler.text;
        var data = JsonUtility.FromJson<JsonAdsData>(jsonText);

        // Загрузка превью картинки
        var previewRequest = UnityWebRequestAssetBundle.GetAssetBundle(data.preview);

        var previewAsync = previewRequest.SendWebRequest();

        yield return previewAsync;

        var previewBundle = ((DownloadHandlerAssetBundle)previewRequest.downloadHandler).assetBundle;

        var previwName = previewBundle.GetAllAssetNames().First();

        var spriteRequest = previewBundle.LoadAssetAsync(previwName, typeof(Sprite));

        yield return spriteRequest;

        onPreviewLoaded?.Invoke((Sprite)spriteRequest.asset);
        onUrlLoaded?.Invoke(data.url);

        // Загрузка основного бандла со спрайтами

        var bundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(data.bundle);

        var async = bundleRequest.SendWebRequest();

        yield return async;

        var bundle = ((DownloadHandlerAssetBundle)bundleRequest.downloadHandler).assetBundle;

        List<Sprite> sprites = new();

        foreach (var name in bundle.GetAllAssetNames())
        {
            var request = bundle.LoadAssetAsync(name, typeof(Sprite));

            yield return request;

            sprites.Add((Sprite)request.asset);
        }

        onSpritesLoaded?.Invoke(sprites);

        Sprites = sprites;
    }

    public struct JsonAdsData
    {
        public string preview;
        public string bundle;
        public string url;
    }
}


