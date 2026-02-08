using System;
using System.Collections.Generic;
using UnityEngine;

namespace TToTT.GameEvents
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "SaveSystem/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private readonly List<Action> _actions = new();

        public void Register(Action action)
        {
            if (action != null && !_actions.Contains(action))
                _actions.Add(action);
        }

        public void Unregister(Action action)
        {
            if (_actions.Contains(action))
                _actions.Remove(action);
        }

        public void Clear()
        {
            _actions.Clear();
        }

        public void Raise()
        {
            foreach (Action action in _actions)
            {
                action?.Invoke();
            }
        }
    }
}