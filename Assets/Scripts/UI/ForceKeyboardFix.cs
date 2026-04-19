using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class ForceKeyboardFix : MonoBehaviour, IPointerClickHandler
{
    private TMP_InputField _inputField;
    private TouchScreenKeyboard _keyboard;

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();

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