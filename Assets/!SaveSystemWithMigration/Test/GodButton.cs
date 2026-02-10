using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GodButton : CustomButton
{
    public event Action<ItemType, int> OnClick;

    [Header(nameof(GodButton))]
    [SerializeField] private ItemType _itemType;
    [SerializeField, Min(0)] private int _count;

    #region Unity API

    private void Awake() => InitButton();

    #endregion

    private void InitButton()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => OnClick?.Invoke(_itemType, _count));
    }
}
