using SpawningSpheresGame.Game.Gameplay.Root.GameplayParams;
using Zenject;

namespace SpawningSpheresGame.Game.Gameplay.Root
{
    public static class GameplayRegistrations
    {
        public static DiContainer GameplayContainer { get; private set; }


        public static void Register(DiContainer container, GameplayEnterParams gameplayEnterParams)
        {
            GameplayContainer = container;
        }
    }
}