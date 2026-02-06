using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void Init()
    {
        Clear();

        Log("* ================================= *");
        Log("* ****************** SAVE * SYSTEM ****************** *");
        Log("* ================================= *");
    }

    public void Log(string text) => _text.text += $"{text}\n";

    public void Clear() => _text.text = string.Empty;
}
