public interface IPersistentData 
{
    int Version { get; set; }
    PlayerData PlayerData { get; set; }

}
