namespace TToTT.SaveSystem.Migration
{
    public class SaveMigration_3_4 : ISaveMigration
    {
        public int FromVersion => 3;
        public int ToVersion => 4;

        public IPersistentData Migrate(IPersistentData oldSave)
        {
            oldSave.PlayerData.Expirience = oldSave.PlayerData.Level * 100;

            oldSave.Version = ToVersion;
            return oldSave;
        }
    }
}