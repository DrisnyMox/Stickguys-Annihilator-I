#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSwitcher
{
    [MenuItem("Stickguys/Scenes/Menu")]
    public static void OpenMenuScene()
    {
        OpenScene("Menu");
    }

    [MenuItem("Stickguys/Scenes/Editor Car")]
    public static void OpenEditorCarScene()
    {
        OpenScene("Editor Car");
    }

    [MenuItem("Stickguys/Saves/Reset All Saves")]
    public static void ResetAllSaves()
    {
        bool confirm = EditorUtility.DisplayDialog(
            "Reset saves",
            "Delete all saved data (PlayerPrefs)? This action cannot be undone.",
            "Delete",
            "Cancel");

        if (!confirm)
        {
            return;
        }

        SaveLoadSystem.DeleteAll();
        SaveLoadSystem.Save();
        Debug.Log("All saves were deleted.");
    }

    private static void OpenScene(string sceneName)
    {
        // Спрашиваем, нужно ли сохранить текущую сцену перед переключением
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // Ищем сцену по имени во всем проекте
            string[] guids = AssetDatabase.FindAssets(sceneName + " t:Scene");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                EditorSceneManager.OpenScene(path);
            }
            else
            {
                Debug.LogError("Сцена '" + sceneName + "' не найдена в проекте!");
            }
        }
    }
}
#endif
