using SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams;
using Zenject;

namespace SpawningSpheresGame.Game.MainMenu.Root
{
    public static class MainMenuRegistrations
    {
        public static DiContainer MainMenuContainer { get; private set; }


        public static void Register(DiContainer container, MainMenuEnterParams mainMenuEnterParams)
        {
            MainMenuContainer = container;
        }
    }
}