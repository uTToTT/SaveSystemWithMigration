namespace TToTT.SaveSystem
{
    public sealed class SaveService
    {
        private IPersistentData _persistentData;
        private IDataProvider _provider;

        public SaveService()
        {
            InitData();
        }

        public void Save() => _provider.Save();
        public void Delete() => _provider.Delete();
        public void Load() => LoadOrInit();

        private void InitData()
        {
            _persistentData = new PersistentData();
            _provider = new DataLocalProvider(_persistentData);
        }

        private void LoadOrInit()
        {
            if (_provider.TryLoad() == false)
                _persistentData.PlayerData = new PlayerData();
        }
    }
}