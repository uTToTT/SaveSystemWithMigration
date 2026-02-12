using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace TToTT.SaveSystem
{
    public sealed class DataLocalProvider : IDataProvider
    {
        public event Action OnSaved;
        public event Action OnLoaded;
        public event Action OnDeleted;

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

        public static string SavePath => Application.persistentDataPath;
        public static string GetErrorPath() => Path.Combine(SavePath, $"{ERROR_FILE_PREFIX}{ERROR_FILE_EXTENSION}");
        public static string GetSavePath() => Path.Combine(SavePath, $"{SAVE_FILE_PREFIX}{SAVE_FILE_EXTENSION}");

        #region API

        public bool TryLoad()
        {
            var primaryPath = GetSavePath();
            var backupPath = primaryPath + SAVE_BACKUP_EXTENSION;
            var tempPath = primaryPath + SAVE_TEMP_EXTENSION;

            try
            {
                if (HasSave(tempPath))
                    File.Delete(tempPath);
            }
            catch { }

            try
            {
                if (HasSave(primaryPath) &&
                    Load(primaryPath))
                {
                    _logger.Log($"Loaded from [{primaryPath}]");
                }
                else if (
                    HasSave(backupPath) &&
                    Load(backupPath))
                {
                    _logger.Log($"Loaded from [{backupPath}]");
                    File.Copy(backupPath, primaryPath, true);
                }
                else return false;

                OnLoaded?.Invoke();
                return true;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                return false;
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

                OnSaved?.Invoke();
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
                var primaryPath = GetSavePath();
                var backupPath = primaryPath + SAVE_BACKUP_EXTENSION;

                File.Delete(primaryPath);
                File.Delete(backupPath);

                OnDeleted?.Invoke();
                _logger.Log($"Deleted [{primaryPath}]");
                _logger.Log($"Deleted [{backupPath}]");
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw;
            }

        }

        #endregion

        private bool HasSave(string path) => File.Exists(path);

        private bool Load(string path)
        {
            var rawData = File.ReadAllText(path);
            var data = JsonConvert.DeserializeObject<PersistentData>(rawData);

            if (data == null ||
                data.PlayerData == null)
                return false;

            _runtimePersistentData.Version = data.Version;
            _runtimePersistentData.PlayerData = data.PlayerData;

            return true;
        }

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
            try
            {
                _logger.Log(ex.ToString());

                var path = GetErrorPath();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.AppendAllText(path, $"[{DateTime.Now}] {ex}\n\n");
            }
            catch { }
        }
    }
}