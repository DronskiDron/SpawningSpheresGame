using Zenject;

namespace SpawningSpheresGame.Game.Common.MVVM.UI
{
    public abstract class UIManager
    {
        protected readonly DiContainer Container;

        protected UIManager(DiContainer container)
        {
            Container = container;
        }
    }
}