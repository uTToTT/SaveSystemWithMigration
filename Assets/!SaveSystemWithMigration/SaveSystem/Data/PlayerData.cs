using System;

[Serializable]
public class PlayerData 
{
    public string Name;
    public float Expirience;
    public int Level;
    public int Money;
    public int Gems;

    public PlayerData()
    {
        Name = "User";
    }
}
