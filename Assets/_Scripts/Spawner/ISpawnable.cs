using System;

namespace _Scripts.Spawner
{
    public interface ISpawnable
    {
        void Spawn();

        void Despawn(Action onComplete);
    }
}