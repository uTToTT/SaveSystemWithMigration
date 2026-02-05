public interface ISaveMigration
{
    int FromVersion { get; }
    int ToVersion { get; }

    IPersistentData Migrate(IPersistentData oldSave);
}
