using System;
using System.Diagnostics;
using System.IO;
using TToTT.SaveSystem;
using UnityEngine;
using UnityEngine.UI;

public class SaveDebugger : MonoBehaviour
{
    [SerializeField] private Button _saveButon;
    [SerializeField] private Button _loadButon;
    [SerializeField] private Button _deleteButon;
    [SerializeField] private Button _corruptButon;
    [SerializeField] private Button _spamButon;
    [SerializeField] private Button _folderButon;

    public void Init()
    {
        _saveButon.Init(SaveClick);
        _loadButon.Init(LoadClick);
        _deleteButon.Init(DeleteClick);
        _corruptButon.Init(CorruptClick);
        _spamButon.Init(SpamClick);
        _folderButon.Init(FolderClick);
    }

    private void SaveClick() => GameManager.Instance.SaveService.Save();
    private void LoadClick() => GameManager.Instance.SaveService.Load();
    private void DeleteClick() => GameManager.Instance.SaveService.Delete();

    private void CorruptClick()
    {
        string fullPath = DataLocalProvider.GetSavePath();

        if (!File.Exists(fullPath))
            return; 

        var fullFile = File.ReadAllText(fullPath);
        var corruptFile = fullFile.Replace("\"", "brackets");
        File.WriteAllText(fullPath, corruptFile);
    }

    private void SpamClick()
    {
        int successCount = 0;
        int failCount = 0;
        long fileSize = 0;

        var path = DataLocalProvider.GetSavePath();
        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < 100; i++)
        {
            try
            {
                GameManager.Instance.SaveService.Save();
                successCount++;

                if (File.Exists(path))
                {
                    var info = new FileInfo(path);
                    fileSize = info.Length;
                }

                GameManager.Instance.MainLogger.Log
                    ($"Save {i + 1}: Success (size: {fileSize} bytes)");
            }
            catch (Exception ex)
            {
                failCount++;
                GameManager.Instance.MainLogger.Log
                    ($"Save {i + 1}: Failed - {ex.Message}");
            }
        }

        stopwatch.Stop();

        GameManager.Instance.MainLogger.Log($"--- Spam Save Summary ---");
        GameManager.Instance.MainLogger.Log($"Total Saves: 100");
        GameManager.Instance.MainLogger.Log($"Successful: {successCount}");
        GameManager.Instance.MainLogger.Log($"Failed: {failCount}");
        GameManager.Instance.MainLogger.Log($"Last save size: {fileSize} bytes");
        GameManager.Instance.MainLogger.Log($"Total time: {stopwatch.ElapsedMilliseconds} ms");
    }
    private void FolderClick() => Application.OpenURL("file://" + DataLocalProvider.SavePath);
}
