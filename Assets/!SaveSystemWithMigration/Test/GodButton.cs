using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GodButton : MonoBehaviour
{
    public event Action<ItemType, int> OnClick;

    [SerializeField] private ItemType _itemType;
    [SerializeField, Min(0)] private int _count;

    private Button _button;

    private void Reset() => InitButton();
    private void Awake() => InitButton();

    private void InitButton()
    {
        if (_button != null) return;

        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => OnClick?.Invoke(_itemType, _count));
    }
}
