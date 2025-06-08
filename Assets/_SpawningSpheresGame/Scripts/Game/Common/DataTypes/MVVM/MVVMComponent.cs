using System;
using R3;

namespace SpawningSpheresGame.Game.Common.DataTypes
{
    public abstract class MVVMComponent : IDisposable
    {
        protected CompositeDisposable Subscriptions = new();

        public abstract void Initialize();
        public virtual void Dispose() => Subscriptions.Dispose();
    }
}