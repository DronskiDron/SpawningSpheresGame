using SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams;

namespace SpawningSpheresGame.Game.Gameplay.Root.GameplayParams
{
    public class GameplayExitParams
    {
        public MainMenuEnterParams MainMenuEnterParams { get; }


        public GameplayExitParams(MainMenuEnterParams mainMenuEnterParams)
        {
            MainMenuEnterParams = mainMenuEnterParams;
        }
    }
}