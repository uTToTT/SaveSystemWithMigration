namespace TToTT.SaveSystem
{
    [System.Serializable]
    public class PlayerData
    {
        private string _name;
        private float _expirience;
        private int _level;
        private int _money;
        private int _gems;

        public PlayerData()
        {
            Name = "User";
            Expirience = 0;
            Level = 1;
            Money = 100;
            Gems = 5;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public float Expirience
        {
            get => _expirience;
            set
            {
                if (value < 0)
                    throw new System.ArgumentOutOfRangeException(nameof(value));

                _expirience = value;
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                if (value < 0)
                    throw new System.ArgumentOutOfRangeException(nameof(value));

                _level = value;
            }
        }

        public int Money
        {
            get => _money;
            set
            {
                if (value < 0)
                    throw new System.ArgumentOutOfRangeException(nameof(value));

                _money = value;
            }
        }

        public int Gems
        {
            get => _gems;
            set
            {
                if (value < 0)
                    throw new System.ArgumentOutOfRangeException(nameof(value));

                _gems = value;
            }
        }
    }
}