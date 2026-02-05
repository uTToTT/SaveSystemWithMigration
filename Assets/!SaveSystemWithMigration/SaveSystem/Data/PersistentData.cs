[System.Serializable]
public class PersistentData : IPersistentData 
{
    public int Version {  get; set; }
    public PlayerData PlayerData { get; set; }
}
