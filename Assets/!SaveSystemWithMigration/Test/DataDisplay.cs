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

    public void Init() => _gameDataUpdated.Register(ShowData);

    public void Display(string message) => _dataText.text = message;
    public void Clear() => _dataText.text = string.Empty;

    private void ShowData()
    {
        StringBuilder sb = new StringBuilder();

        var data = GameManager.Instance.SaveService.Data;
        var playerData = data.PlayerData;

        sb.AppendLine($"Version [{data.Version}]");
        sb.AppendLine();
        sb.AppendLine($"Name: [{playerData.Name}]");
        sb.AppendLine($"Level: [{playerData.Level}]");
        sb.AppendLine($"Expirience: [{playerData.Expirience}]");
        sb.AppendLine($"Money: [{playerData.Money}]");
        sb.AppendLine($"Gems: [{playerData.Gems}]");

        Display(sb.ToString());
    }
}
