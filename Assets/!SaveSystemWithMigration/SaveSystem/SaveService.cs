using TToTT.SaveSystem.Migration;

namespace TToTT.SaveSystem
{
    public sealed class SaveService
    {
        private readonly int _currentSaveVersion;

        private IPersistentData _runtimePersistentData;
        private IDataProvider _provider;
        private ILogger _logger;

        private SaveMigrationRegistry _registry;

        public SaveService(
            int currentSaveVersion,
            ILogger logger,
            IPersistentData persistentData,
            IDataProvider dataProvider)
        {
            _currentSaveVersion = currentSaveVersion;

            InitDependecies(logger);
            InitData(persistentData, dataProvider);
        }

        public void Save() => _provider.Save();
        public void Delete() => _provider.Delete();
        public void Load() => LoadOrInit();

        private void InitData(IPersistentData persistent, IDataProvider provider)
        {
            _runtimePersistentData = persistent;
            _provider = provider;
        }

        private void InitDependecies(ILogger logger)
        {
            _logger = logger;

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
                var oldVersion = save.Version;
                var migration = _registry.GetMigration(save.Version);
                save = migration.Migrate(save);

                if (save.Version != _currentSaveVersion)
                {
                    throw new System.ArgumentException
                        ($"Save [{save.Version}] do not migrated to newest version [{_currentSaveVersion}]");
                }

                _logger.Log($"SaveMigration {oldVersion}->{save.Version}");
            }

            return save;
        }
    }
}