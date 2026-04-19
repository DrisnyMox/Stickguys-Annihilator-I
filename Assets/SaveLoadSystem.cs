using UnityEngine;
using YG;

public static class SaveLoadSystem {

    public const string KeyCoins = "coins";
    public const string KeyGears = "gears";
    public const string KeyFirstTNT = "fTNT";
    public const string KeySecondTNT = "sTNT";
    public const string KeyGameData = "gmdata";
    public const string KeyGameAuto = "gmauto";
    public const string KeyEditorCarsData = "editorCarsData";
    public const string KeyEditor = "editor";
    public const string KeySlow = "stSlow";
    public const string KeyDistance = "stDist";
    public const string KeyBlood = "stBlood";
    public const string KeyLanguage = "stLang";
    public const string KeyTNTBonus = "TNTBonus";
    public const string KeyTooltipTNT = "tooltipTNT";
    public const string KeyChmos = "chmos";
    public const string KeySkidko = "skidko";

    static bool IsWebYGActive() {
#if UNITY_WEBGL
        return YandexGame.Instance != null;
#else
        return false;
#endif
    }

    static void SaveProgressYG() {
#if UNITY_WEBGL
        if (YandexGame.Instance != null && YandexGame.SDKEnabled) {
            YandexGame.SaveProgress();
        }
#endif
    }

    public static bool HasKey(string key) {
        return PlayerPrefs.HasKey(key);
    }

    public static void DeleteKey(string key) {
        PlayerPrefs.DeleteKey(key);
    }

    public static void DeleteAll() {
        PlayerPrefs.DeleteAll();
    }

    public static void Save() {
        PlayerPrefs.Save();
    }

    public static int LoadCoins() {
        return PlayerPrefs.GetInt(KeyCoins, 0);
    }

    public static void SaveCoins(int value) {
        PlayerPrefs.SetInt(KeyCoins, value);
        if (IsWebYGActive()) {
            YandexGame.savesData.coins = value;
            SaveProgressYG();
        }
        Save();
    }

    public static int LoadGears(int defaultValue) {
        return PlayerPrefs.GetInt(KeyGears, defaultValue);
    }

    public static void SaveGears(int value) {
        PlayerPrefs.SetInt(KeyGears, value);
        if (IsWebYGActive()) {
            YandexGame.savesData.gears = value;
            SaveProgressYG();
        }
        Save();
    }

    public static void LoadTNT(out string first, out string second, string defaultFirst, string defaultSecond) {
        first = PlayerPrefs.GetString(KeyFirstTNT, defaultFirst);
        second = PlayerPrefs.GetString(KeySecondTNT, defaultSecond);
    }

    public static void SaveTNT(string first, string second) {
        PlayerPrefs.SetString(KeyFirstTNT, first);
        PlayerPrefs.SetString(KeySecondTNT, second);
        if (IsWebYGActive()) {
            YandexGame.savesData.firstTNT = first;
            YandexGame.savesData.secondTNT = second;
            SaveProgressYG();
        }
        Save();
    }

    public static void SaveGameData(string data) {
        PlayerPrefs.SetString(KeyGameData, data);
        if (IsWebYGActive()) {
            YandexGame.savesData.gameData = data;
            SaveProgressYG();
        }
    }

    public static string LoadGameData() {
        return PlayerPrefs.GetString(KeyGameData, string.Empty);
    }

    public static void SaveAutosData(string data) {
        PlayerPrefs.SetString(KeyGameAuto, data);
        if (IsWebYGActive()) {
            YandexGame.savesData.autosData = data;
            SaveProgressYG();
        }
    }

    public static string LoadAutosData() {
        return PlayerPrefs.GetString(KeyGameAuto, string.Empty);
    }

    public static void SaveEditorCarsData(string data) {
        PlayerPrefs.SetString(KeyEditorCarsData, data ?? string.Empty);
        if (IsWebYGActive()) {
            YandexGame.savesData.editorCarsData = data ?? string.Empty;
            SaveProgressYG();
        }
        Save();
    }

    public static string LoadEditorCarsData() {
        return PlayerPrefs.GetString(KeyEditorCarsData, string.Empty);
    }

    public static void SaveEditorState(bool isOpen) {
        PlayerPrefs.SetString(KeyEditor, isOpen.ToString());
        if (IsWebYGActive()) {
            YandexGame.savesData.editorIsOpen = isOpen;
            SaveProgressYG();
        }
        Save();
    }

    public static bool LoadEditorState(bool defaultValue) {
        return bool.Parse(PlayerPrefs.GetString(KeyEditor, defaultValue.ToString()));
    }

    public static void SaveSlowMo(bool value) {
        PlayerPrefs.SetString(KeySlow, value.ToString());
        if (IsWebYGActive()) {
            YandexGame.savesData.slowMo = value;
            SaveProgressYG();
        }
        Save();
    }

    public static bool LoadSlowMo(bool defaultValue) {
        return bool.Parse(PlayerPrefs.GetString(KeySlow, defaultValue.ToString()));
    }

    public static void SaveDistance(float value) {
        PlayerPrefs.SetFloat(KeyDistance, value);
        if (IsWebYGActive()) {
            YandexGame.savesData.distance = value;
            SaveProgressYG();
        }
        Save();
    }

    public static float LoadDistance(float defaultValue) {
        return PlayerPrefs.GetFloat(KeyDistance, defaultValue);
    }

    public static void SaveBlood(float value) {
        PlayerPrefs.SetFloat(KeyBlood, value);
        if (IsWebYGActive()) {
            YandexGame.savesData.blood = value;
            SaveProgressYG();
        }
        Save();
    }

    public static float LoadBlood(float defaultValue) {
        return PlayerPrefs.GetFloat(KeyBlood, defaultValue);
    }

    public static void SaveLanguage(int value) {
        PlayerPrefs.SetInt(KeyLanguage, value);
        if (IsWebYGActive()) {
            YandexGame.savesData.languageIndex = value;
            SaveProgressYG();
        }
        Save();
    }

    public static int LoadLanguage(int defaultValue) {
        return PlayerPrefs.GetInt(KeyLanguage, defaultValue);
    }

    public static void SaveString(string key, string value, bool saveNow) {
        PlayerPrefs.SetString(key, value);
        if (IsWebYGActive()) {
            if (key == KeyTooltipTNT) YandexGame.savesData.tooltipTNT = value;
            else if (key == KeyChmos) YandexGame.savesData.chmos = value;
            else if (key == KeySkidko) YandexGame.savesData.skidko = value;
            else if (key == "unlocksColor") YandexGame.savesData.unlocksColor = value;
            else if (key == "bloodActive") YandexGame.savesData.bloodActive = bool.Parse(value);

            SaveProgressYG();
        }
        if (saveNow) {
            Save();
        }
    }

    public static void SaveInt(string key, int value, bool saveNow) {
        PlayerPrefs.SetInt(key, value);
        if (IsWebYGActive()) {
            if (key == KeyTNTBonus) YandexGame.savesData.tntBonus = value;// == 1 ? false : true;
            else if (key == "boneColor") YandexGame.savesData.boneColorIndex = value;

            SaveProgressYG();
        }
        if (saveNow) {
            Save();
        }
    }
}
