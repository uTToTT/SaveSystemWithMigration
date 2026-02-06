namespace TToTT.SaveSystem.Migration
{
    public class SaveMigration_2_3 : ISaveMigration
    {
        private const string OLD_DEFAULT_NAME = "User";
        private const string NEW_DEFAULT_NAME = "Player";

        public int FromVersion => 2;
        public int ToVersion => 3;

        public IPersistentData Migrate(IPersistentData oldSave)
        {
            if (oldSave.PlayerData.Name.Equals(OLD_DEFAULT_NAME))
                oldSave.PlayerData.Name = NEW_DEFAULT_NAME;

            oldSave.Version = ToVersion;
            return oldSave;
        }
    }
}