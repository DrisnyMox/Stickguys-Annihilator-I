using UnityEngine;

public static class SaveLoadSystem {

    public const string KeyCoins = "coins";
    public const string KeyGears = "gears";
    public const string KeyFirstTNT = "fTNT";
    public const string KeySecondTNT = "sTNT";
    public const string KeyGameData = "gmdata";
    public const string KeyGameAuto = "gmauto";
    public const string KeyEditor = "editor";
    public const string KeySlow = "stSlow";
    public const string KeyDistance = "stDist";
    public const string KeyBlood = "stBlood";
    public const string KeyLanguage = "stLang";
    public const string KeyTNTBonus = "TNTBonus";
    public const string KeyTooltipTNT = "tooltipTNT";
    public const string KeyChmos = "chmos";
    public const string KeySkidko = "skidko";

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
        Save();
    }

    public static int LoadGears(int defaultValue) {
        return PlayerPrefs.GetInt(KeyGears, defaultValue);
    }

    public static void SaveGears(int value) {
        PlayerPrefs.SetInt(KeyGears, value);
        Save();
    }

    public static void LoadTNT(out string first, out string second, string defaultFirst, string defaultSecond) {
        first = PlayerPrefs.GetString(KeyFirstTNT, defaultFirst);
        second = PlayerPrefs.GetString(KeySecondTNT, defaultSecond);
    }

    public static void SaveTNT(string first, string second) {
        PlayerPrefs.SetString(KeyFirstTNT, first);
        PlayerPrefs.SetString(KeySecondTNT, second);
        Save();
    }

    public static void SaveGameData(string data) {
        PlayerPrefs.SetString(KeyGameData, data);
    }

    public static string LoadGameData() {
        return PlayerPrefs.GetString(KeyGameData, string.Empty);
    }

    public static void SaveAutosData(string data) {
        PlayerPrefs.SetString(KeyGameAuto, data);
    }

    public static string LoadAutosData() {
        return PlayerPrefs.GetString(KeyGameAuto, string.Empty);
    }

    public static void SaveEditorState(bool isOpen) {
        PlayerPrefs.SetString(KeyEditor, isOpen.ToString());
        Save();
    }

    public static bool LoadEditorState(bool defaultValue) {
        return bool.Parse(PlayerPrefs.GetString(KeyEditor, defaultValue.ToString()));
    }

    public static void SaveSlowMo(bool value) {
        PlayerPrefs.SetString(KeySlow, value.ToString());
        Save();
    }

    public static bool LoadSlowMo(bool defaultValue) {
        return bool.Parse(PlayerPrefs.GetString(KeySlow, defaultValue.ToString()));
    }

    public static void SaveDistance(float value) {
        PlayerPrefs.SetFloat(KeyDistance, value);
        Save();
    }

    public static float LoadDistance(float defaultValue) {
        return PlayerPrefs.GetFloat(KeyDistance, defaultValue);
    }

    public static void SaveBlood(float value) {
        PlayerPrefs.SetFloat(KeyBlood, value);
        Save();
    }

    public static float LoadBlood(float defaultValue) {
        return PlayerPrefs.GetFloat(KeyBlood, defaultValue);
    }

    public static void SaveLanguage(int value) {
        PlayerPrefs.SetInt(KeyLanguage, value);
        Save();
    }

    public static int LoadLanguage(int defaultValue) {
        return PlayerPrefs.GetInt(KeyLanguage, defaultValue);
    }

    public static void SaveString(string key, string value, bool saveNow) {
        PlayerPrefs.SetString(key, value);
        if (saveNow) {
            Save();
        }
    }

    public static void SaveInt(string key, int value, bool saveNow) {
        PlayerPrefs.SetInt(key, value);
        if (saveNow) {
            Save();
        }
    }
}
