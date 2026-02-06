using TToTT.SaveSystem.Migration;

namespace TToTT.SaveSystem
{
    public sealed class SaveService
    {
        private int _currentSaveVersion;

        private IPersistentData _runtimePersistentData;
        private IDataProvider _provider;

        private SaveMigrationRegistry _registry;

        public SaveService(int currentSaveVersion)
        {
            InitDependecies();
            InitData();

            _currentSaveVersion = currentSaveVersion;
        }

        public void Save() => _provider.Save();
        public void Delete() => _provider.Delete();
        public void Load() => LoadOrInit();

        private void InitData()
        {
            _runtimePersistentData = new PersistentData();
            _provider = new DataLocalProvider(_runtimePersistentData);
        }

        private void InitDependecies()
        {
            var migrations = new ISaveMigration[]
            {
                new SaveMigration_1_2(),
                new SaveMigration_2_3(),
                new SaveMigration_3_4(),
            };

            _registry = new SaveMigrationRegistry(migrations);
        }

        private void LoadOrInit()
        {
            if (_provider.TryLoad() == false)
            {
                _runtimePersistentData.PlayerData = new PlayerData();
                _runtimePersistentData.Version = _currentSaveVersion;
            }

            MigrateIfNeeded(_runtimePersistentData);
        }

        private IPersistentData MigrateIfNeeded(IPersistentData save)
        {
            while (save.Version < _currentSaveVersion)
            {
                var migration = _registry.GetMigration(save.Version);
                save = migration.Migrate(save);
            }

            return save;
        }
    }
}