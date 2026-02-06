namespace TToTT.SaveSystem.Migration
{
    public class SaveMigration_1_2 : ISaveMigration
    {
        public int FromVersion => 1;
        public int ToVersion => 2;

        public IPersistentData Migrate(IPersistentData oldSave)
        {
            var exp = oldSave.PlayerData.Expirience;

            if (exp > 0)
            {
                oldSave.PlayerData.Level = (int)(oldSave.PlayerData.Expirience / 100);
            }

            oldSave.Version = ToVersion;
            return oldSave;
        }
    }
}