using TToTT.SaveSystem;
using UnityEngine;

public class GodConsole : MonoBehaviour
{
    [SerializeField] private GodButton[] _buttons;

    public void Init()
    {
        foreach (var button in _buttons)
        {
            button.OnClick += GiveItem;
        }
    }

    private void GiveItem(ItemType itemType, int count = 1)
    {
        var data = GameManager.Instance.SaveService.Data.PlayerData;

        if (data == null) return;

        switch (itemType)
        {
            case ItemType.None:
                break;
            case ItemType.Gem:
                data.Gems += count;
                break;
            case ItemType.Money:
                data.Money += count;
                break;
            case ItemType.Expirience:
                data.Expirience += count;
                break;
            case ItemType.Level:
                data.Level += count;
                break;

            default:
                return;
        }

        GameManager.Instance.EventManager.GameDataUpdated();
    }
}
