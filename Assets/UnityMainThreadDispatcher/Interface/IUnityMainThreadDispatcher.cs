using System;

namespace UnityMainThreadDispatcher
{
    public interface IUnityMainThreadDispatcher
    {
        void Enqueue(Action action);
    }
}