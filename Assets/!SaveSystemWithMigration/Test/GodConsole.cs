using TToTT.SaveSystem;
using UnityEngine;

public class GodConsole : MonoBehaviour
{
    [SerializeField] private GodButton[] _buttons;

    private PlayerData _data;

    public void Init()
    {
        _data = GameManager.Instance.SaveService.Data.PlayerData;

        foreach (var button in _buttons)
        {
            button.OnClick += GiveItem;
        }
    }

    private void GiveItem(ItemType itemType, int count = 1)
    {
        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Gem:
                _data.Gems += count;
                break;
            case ItemType.Money:
                _data.Money += count;
                break;
            case ItemType.Expirience:
                _data.Expirience += count;
                break;
            case ItemType.Level:
                _data.Level += count;
                break;
            default:
                break;
        }
    }
}
