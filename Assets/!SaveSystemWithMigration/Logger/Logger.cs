using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour, ILogger
{
    [SerializeField] private TMP_Text _text;

    #region Init

    public void Init() => Clear();

    #endregion

    public void Log(string text) => _text.text += $"- {text}\n";
    public void Clear() => _text.text = string.Empty;
}
