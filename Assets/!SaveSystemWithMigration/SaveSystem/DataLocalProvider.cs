using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace TToTT.SaveSystem
{
    public sealed class DataLocalProvider : IDataProvider
    {
        private const string SAVE_FILE_PREFIX = "PlayerSave";
        private const string ERROR_FILE_PREFIX = "error";

        private const string SAVE_FILE_EXTENSION = ".json";
        private const string ERROR_FILE_EXTENSION = ".txt";
        private const string SAVE_TEMP_EXTENSION = ".tmp";
        private const string SAVE_BACKUP_EXTENSION = ".bak";

        private IPersistentData _runtimePersistentData;
        private ILogger _logger;

        public DataLocalProvider(ILogger logger, IPersistentData persistentData)
        {
            _runtimePersistentData = persistentData;
            _logger = logger;
        }

        private string SavePath => Application.persistentDataPath;
        private string GetErrorPath() => Path.Combine(SavePath, $"{ERROR_FILE_PREFIX}{ERROR_FILE_EXTENSION}");
        private string GetSavePath() => Path.Combine(SavePath, $"{SAVE_FILE_PREFIX}{SAVE_FILE_EXTENSION}");

        #region API

        public bool TryLoad()
        {
            try
            {
                if (IsAlreadyExists() == false)
                    return false;

                var path = GetSavePath();
                var rawData = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<PersistentData>(rawData);

                if (data == null)
                {
                    var backupPath = path + SAVE_BACKUP_EXTENSION;

                    if (File.Exists(backupPath))
                    {
                        rawData = File.ReadAllText(backupPath);
                        data = JsonConvert.DeserializeObject<PersistentData>(rawData);

                        if (data == null)
                        {
                            return false;
                        }
                    }
                }

                _runtimePersistentData.Version = data.Version;
                _runtimePersistentData.PlayerData = data.PlayerData;

                _logger.Log($"Loaded [{path}]");

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
                var json = Serialize(_runtimePersistentData);

                var finalPath = GetSavePath();
                var tempPath = finalPath + SAVE_TEMP_EXTENSION;
                var backupPath = finalPath + SAVE_BACKUP_EXTENSION;

                using (var fs = new FileStream(
                    tempPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None))
                using (var writer = new StreamWriter(fs))
                {
                    writer.Write(json);
                    writer.Flush();
                    fs.Flush(true);
                }

                if (File.Exists(finalPath))
                    File.Replace(tempPath, finalPath, backupPath);
                else
                    File.Move(tempPath, finalPath);

                _logger.Log($"Saved [{finalPath}]");
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
                var path = GetSavePath();
                File.Delete(path);

                _logger.Log($"Deleted [{path}]");
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }

        }

        #endregion

        private bool IsAlreadyExists() => File.Exists(GetSavePath());

        private string Serialize(IPersistentData data)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            var json = JsonConvert.SerializeObject(data, Formatting.Indented, jsonSettings);
            return json;
        }

        private void WriteErrorLog(Exception ex)
        {
            _logger.Log(ex.ToString());
            File.AppendAllText(GetErrorPath(), $"[{DateTime.Now}] {ex}\n\n");
        }
    }
}