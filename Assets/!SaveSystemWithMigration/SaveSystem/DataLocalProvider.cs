using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public sealed class DataLocalProvider : IDataProvider
{
    private const string SAVE_FILE_PREFIX = "PlayerSave";
    private const string ERROR_FILE_PREFIX = "error";
    private const string SAVE_FILE_EXTENSION = ".json";
    private const string ERROR_FILE_EXTENSION = ".txt";

    private IPersistentData _persistentData;

    public DataLocalProvider(IPersistentData persistentData)
    {
        _persistentData = persistentData;
    }

    private string SavePath => Application.persistentDataPath;
    private string ErrorLogPath => Path.Combine(SavePath, $"{ERROR_FILE_PREFIX}{ERROR_FILE_EXTENSION}");
    private string GetFullPath() => Path.Combine(SavePath, $"{SAVE_FILE_PREFIX}{SAVE_FILE_EXTENSION}");

    public bool TryLoad()
    {
        try
        {
            if (IsAlreadyExists() == false)
                return false;

            var path = GetFullPath();
            var data = File.ReadAllText(path);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(data);

            _persistentData.PlayerData = playerData;

            Debug.Log($"Loaded [{path}]");

            return true;
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }
    }

    public void Save()
    {
        try
        {
            var path = GetFullPath();
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(_persistentData.PlayerData, Formatting.Indented, jsonSettings);
            File.WriteAllText(path, json);

            Debug.Log($"Saved [{path}]");
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }
    }

    public void Delete()
    {
        try
        {
            var path = GetFullPath();
            File.Delete(path);

            Debug.Log($"Deleted [{path}]");
        }
        catch (Exception ex)
        {
            WriteErrorLog(ex);
            throw;
        }

    }

    private bool IsAlreadyExists() => File.Exists(GetFullPath());

    private void WriteErrorLog(Exception ex)
    {
        Debug.LogException(ex);
        File.AppendAllText(ErrorLogPath, $"[{DateTime.Now}] {ex}\n\n");
    }
}
