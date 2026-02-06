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
        }

        public ISaveMigration GetMigration(int fromVersion)
        {
            if (!_migrations.TryGetValue(fromVersion, out var migration))
                throw new System.InvalidOperationException
                    ($"No migration found from version {fromVersion}");

            return migration;
        }
    }
}