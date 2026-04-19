using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Подключаем классический UI вместо TMPro

[RequireComponent(typeof(InputField))] // Меняем тип требуемого компонента
public class ForceKeyboardLegacyFix : MonoBehaviour, IPointerClickHandler
{
    private InputField _inputField; // Меняем тип переменной
    private TouchScreenKeyboard _keyboard;

    private void Awake()
    {
        _inputField = GetComponent<InputField>(); // Получаем классический InputField

        _inputField.shouldHideMobileInput = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ForceOpenKeyboard());
    }

    private IEnumerator ForceOpenKeyboard()
    {
        yield return new WaitForEndOfFrame();

        _inputField.ActivateInputField();

        if (TouchScreenKeyboard.isSupported)
        {
            _keyboard = TouchScreenKeyboard.Open(_inputField.text, TouchScreenKeyboardType.Default);
        }
    }

    private void Update()
    {
        if (_keyboard != null && _keyboard.status == TouchScreenKeyboard.Status.Visible)
        {
            _inputField.text = _keyboard.text;
        }
    }
}