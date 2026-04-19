using UnityEngine;
using YG;

public class YGSaveSync : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        if (FindObjectOfType<YGSaveSync>() != null)
            return;

        GameObject syncObject = new GameObject("YGSaveSync");
        DontDestroyOnLoad(syncObject);
        syncObject.AddComponent<YGSaveSync>();
    }

    void OnEnable()
    {
        YandexGame.GetDataEvent += SyncFromYGToPlayerPrefs;
    }

    void OnDisable()
    {
        YandexGame.GetDataEvent -= SyncFromYGToPlayerPrefs;
    }

    void Start()
    {
        if (YandexGame.SDKEnabled)
            SyncFromYGToPlayerPrefs();
    }

    void SyncFromYGToPlayerPrefs()
    {
        SavesYG data = YandexGame.savesData;

        PlayerPrefs.SetInt(SaveLoadSystem.KeyCoins, data.coins);
        PlayerPrefs.SetInt(SaveLoadSystem.KeyGears, data.gears);
        PlayerPrefs.SetString(SaveLoadSystem.KeyFirstTNT, data.firstTNT ?? string.Empty);
        PlayerPrefs.SetString(SaveLoadSystem.KeySecondTNT, data.secondTNT ?? string.Empty);
        PlayerPrefs.SetString(SaveLoadSystem.KeyGameData, data.gameData ?? string.Empty);
        PlayerPrefs.SetString(SaveLoadSystem.KeyGameAuto, data.autosData ?? string.Empty);
        PlayerPrefs.SetString(SaveLoadSystem.KeyEditor, data.editorIsOpen.ToString());
        PlayerPrefs.SetString(SaveLoadSystem.KeySlow, data.slowMo.ToString());
        PlayerPrefs.SetFloat(SaveLoadSystem.KeyDistance, data.distance);
        PlayerPrefs.SetFloat(SaveLoadSystem.KeyBlood, data.blood);
        PlayerPrefs.SetInt(SaveLoadSystem.KeyLanguage, data.languageIndex);
        PlayerPrefs.SetInt(SaveLoadSystem.KeyTNTBonus, data.tntBonus);
        PlayerPrefs.SetString(SaveLoadSystem.KeyTooltipTNT, data.tooltipTNT ?? string.Empty);
        PlayerPrefs.SetString(SaveLoadSystem.KeyChmos, data.chmos ?? string.Empty);
        PlayerPrefs.SetString(SaveLoadSystem.KeySkidko, data.skidko ?? string.Empty);

        PlayerPrefs.SetInt("boneColor", data.boneColorIndex);
        PlayerPrefs.SetString("unlocksColor", data.unlocksColor ?? string.Empty);
        PlayerPrefs.SetString("bloodActive", data.bloodActive.ToString());

        PlayerPrefs.Save();
    }
}
