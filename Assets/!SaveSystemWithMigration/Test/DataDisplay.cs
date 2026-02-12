using System.Text;
using System.Xml.Linq;
using TMPro;
using TToTT.GameEvents;
using UnityEngine;
using UnityEngine.UI;

public class DataDisplay : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private GameEvent _gameDataUpdated;

    [Header("UI")]
    [SerializeField] private TMP_Text _dataText;

    public void Init()
    {
        ShowData();
        _gameDataUpdated.Register(ShowData);
    }

    public void Display(string message) => _dataText.text = message;
    public void Clear() => _dataText.text = string.Empty;

    private void ShowData()
    {
        const string NULL = "NULL";

        string version = NULL;
        string name = NULL;
        string level = NULL;
        string expirience = NULL;
        string money = NULL;
        string gems = NULL;

        StringBuilder sb = new StringBuilder();

        var data = GameManager.Instance.SaveService.Data;

        if (data != null && data.PlayerData != null)
        {
            var playerData = data.PlayerData;

            version = data.Version.ToString();
            name = playerData.Name;
            level = playerData.Level.ToString();
            expirience = playerData.Expirience.ToString();
            money = playerData.Money.ToString();
            gems = playerData.Gems.ToString();
        }

        sb.AppendLine($"Version [{version}]");
        sb.AppendLine();
        sb.AppendLine($"Name: [{name}]");
        sb.AppendLine($"Level: [{level}]");
        sb.AppendLine($"Expirience: [{expirience}]");
        sb.AppendLine($"Money: [{money}]");
        sb.AppendLine($"Gems: [{gems}]");

        Display(sb.ToString());
    }
}
