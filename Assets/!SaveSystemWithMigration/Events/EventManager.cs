using UnityEngine;

namespace TToTT.GameEvents
{
    public class EventManager : MonoBehaviour
    {
        [Header("Game data")]
        [SerializeField] private GameEvent _gameDataSaved;
        [SerializeField] private GameEvent _gameDataDeleted;
        [SerializeField] private GameEvent _gameDataLoaded;

        #region Game data

        public void GameDataSaved() => _gameDataSaved.Raise();
        public void GameDataDeleted() => _gameDataDeleted.Raise();
        public void GameDataLoaded() => _gameDataLoaded.Raise();

        #endregion
    }
}