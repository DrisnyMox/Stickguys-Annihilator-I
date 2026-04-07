using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CrossAdDownloader : MonoBehaviour
{

    [SerializeField] string pathJson;
    [SerializeField] Image adView;
    [SerializeField] Button btnView;
    [SerializeField] GameObject[] adsLabels;

    public static string Url;
    public static Sprite Preview;
    public static BundleAdsLoader bundleLoader;
    public static List<Sprite> Sprites;

    private void Start()
    {
        InitLoader();
        SetVisible(false);

        adView.enabled = false;

        bundleLoader.onUrlLoaded += Url_Loaded;
        bundleLoader.onPreviewLoaded += Preview_Loaded;
        bundleLoader.onSpritesLoaded += MainBundle_Loaded;

        if (Preview)
        {
            adView.sprite = Preview;
            adView.enabled = true;
        }

        if (bundleLoader.Sprites != null)
        {
            Sprites = bundleLoader.Sprites;
            StartCoroutine(SpriteSwitch());
        }
    }

    private void MainBundle_Loaded(List<Sprite> sprites)
    {
        Sprites = sprites;
        
        StartCoroutine(SpriteSwitch());
    }

    private void Preview_Loaded(Sprite icon)
    {
        adView.enabled = true;
        adView.sprite = icon;
        
        Preview = icon;

        SetVisible(true);
    }

    private void Url_Loaded(string url)
    {
        Url = url;
        btnView.onClick.AddListener(() => Application.OpenURL(url));
    }

    IEnumerator SpriteSwitch()
    {
        for (int i = 0; i < Sprites.Count; i++)
        {
            yield return new WaitForSeconds(0.03f);

            adView.sprite = Sprites[i];
        }

        StartCoroutine(SpriteSwitch());
    }

    private void InitLoader()
    {
        if (bundleLoader)
            return;

        var loader = new GameObject("Bundle Ads Loader");
        bundleLoader = loader.AddComponent<BundleAdsLoader>();
        bundleLoader.pathJson = pathJson;
    }

    void SetVisible(bool value)
    {
        foreach (var item in adsLabels)
        {
            item.SetActive(value);
        }
    }
}
