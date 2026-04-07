using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class loadingZeyddyVideo : MonoBehaviour
{
    [SerializeField] Image adView;
    [SerializeField] Button btnVideo;

    List<Sprite> sprites = new();

    private IEnumerator Start()
    {
        adView.gameObject.SetActive(false);

        if (sprites.Count == 0)
        {
            var url = "https://drive.google.com/uc?export=download&id=1qx_wc2YVSIBo5hlF7BMnYyk_KEbcY9bQ";
            //var url = "https://drive.google.com/uc?export=download&id=15eHfpzGvBZIZC_K637HDEAR7KfeYl0Af";

            var bundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);

            var async = bundleRequest.SendWebRequest();

            yield return async;

            var bundle = ((DownloadHandlerAssetBundle)bundleRequest.downloadHandler).assetBundle;


            foreach (var name in bundle.GetAllAssetNames())
            {
                var request = bundle.LoadAssetAsync(name, typeof(Sprite));

                yield return request;

                sprites.Add((Sprite)request.asset);
            }
        }

        btnVideo.onClick.AddListener(Video_Clicked);
        StartCoroutine(SpriteSwitch());
        adView.gameObject.SetActive(true);

    }

    private void Video_Clicked()
    {
        Application.OpenURL("https://youtu.be/xDm8TnrsxJ0");
    }

    IEnumerator SpriteSwitch()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            yield return new WaitForSeconds(0.03f);

            adView.sprite = sprites[i];
        }

        StartCoroutine(SpriteSwitch());
    }
}
