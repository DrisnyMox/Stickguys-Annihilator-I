using UnityEngine;
using UnityEngine.UI; // Используем стандартный UI

public class WebGLInputList : MonoBehaviour
{
    public InputField[] inputFields; // Массив обычных InputField
    private TouchScreenKeyboard[] keyboards;

#if UNITY_WEBGL
    void Start()
    {
        keyboards = new TouchScreenKeyboard[inputFields.Length];
    }

    void Update()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i].isFocused && (keyboards[i] == null || !keyboards[i].active))
            {
                keyboards[i] = TouchScreenKeyboard.Open(inputFields[i].text, TouchScreenKeyboardType.Default);
            }

            if (keyboards[i] != null && keyboards[i].active)
            {
                inputFields[i].text = keyboards[i].text;
            }
        }
    }
#endif
}