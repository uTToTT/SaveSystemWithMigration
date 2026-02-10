using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Button _button;
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    [Header("Font")]
    [SerializeField] private Color _defaultFontColor = Color.white;
    [SerializeField] private Color _highlightFontColor = Color.white;

    [Header("Background")]
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _highlightColor = Color.white;

    #region Unity API

    private void Reset()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    #endregion

    public void OnPointerEnter(PointerEventData eventData) => Highlight();
    public void OnPointerExit(PointerEventData eventData) => Unhighlight();

    private void Highlight()
    {
        _image.color = _highlightColor;
        _text.color = _highlightFontColor;
    }

    private void Unhighlight()
    {
        _image.color = _defaultColor;
        _text.color = _defaultFontColor;
    }
}
