using System.Collections.Generic;
using System.Linq;

namespace TToTT.SaveSystem.Migration
{
    public sealed class SaveMigrationRegistry
    {
        private readonly Dictionary<int, ISaveMigration> _migrations = new();

        public SaveMigrationRegistry(IEnumerable<ISaveMigration> migrations)
        {
            _migrations = migrations.ToDictionary(m => m.FromVersion);

            ValidateMigrations();
        }

        public ISaveMigration GetMigration(int fromVersion)
        {
            if (!_migrations.TryGetValue(fromVersion, out var migration))
                throw new System.InvalidOperationException
                    ($"No migration found from version {fromVersion}");

            return migration;
        }

        private void ValidateMigrations()
        {
            if (_migrations.Count <= 0) return;

            var oldestVersion = int.MaxValue;
            var newestVersion = int.MinValue;

            foreach (var migration in _migrations)
            {
                if (migration.Key != migration.Value.FromVersion)
                {
                    throw new System.ArgumentException
                        ($"Migration with wrong key version [{migration.Key}]:[{migration.Value.FromVersion}]");
                }

                oldestVersion = migration.Key < oldestVersion ? migration.Key : oldestVersion;
                newestVersion = migration.Key > newestVersion ? migration.Key : newestVersion;
            }

            int pointer = oldestVersion;

            while (pointer < newestVersion)
            {
                if (_migrations.TryGetValue(pointer, out var migration))
                {
                    pointer = migration.ToVersion;
                }
                else
                {
                    throw new System.ArgumentException($"Not found migration with version [{pointer}]");
                }
            }

            if (pointer != newestVersion)
            {
                throw new System.ArgumentException($"Not found migration to newest version [{newestVersion}]");
            }
        }
    }
}