using System.Collections.Generic;
using System;
using UnityEngine;

namespace UnityMainThreadDispatcher
{
    public class MainDispatcher : MonoBehaviour, IUnityMainThreadDispatcher
    {
        private static MainDispatcher _instance;

        private readonly Queue<Action> _queuedActions = new Queue<Action>();

        public static MainDispatcher Instance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainDispatcher>();

                if (_instance == null)
                {
                    var go = new GameObject("UnityMainThreadDispatcher");
                    _instance = go.AddComponent<MainDispatcher>();
                }
            }

            return _instance;
        }

        public void Enqueue(Action action)
        {
            lock (_queuedActions)
            {
                _queuedActions.Enqueue(action);
            }
        }

        private void Update()
        {
            while (_queuedActions.Count > 0)
            {
                Action action = null;

                lock (_queuedActions)
                {
                    if (_queuedActions.Count > 0)
                    {
                        action = _queuedActions.Dequeue();
                    }
                }

                action?.Invoke();
            }
        }
    }

}