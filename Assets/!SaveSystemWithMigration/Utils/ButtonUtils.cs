using System;
using UnityEngine.UI;

public static class ButtonUtils
{
    public static void Init(this Button button, params Action[] actions)
    {
        button.onClick.RemoveAllListeners();

        foreach (var act in actions)
        {
            if (act == null) continue;

            button.onClick.AddListener(() => act());
        }
    }
}
