using R3;
using SpawningSpheresGame.Game.State.Root;

namespace SpawningSpheresGame.Game.State
{
    public interface IGameStateProvider
    {
        public GameState GameState { get; }
        public GameSettingsState SettingsState { get; }
        public Observable<Unit> OnPreSave { get; }

        public Observable<GameState> LoadGameState();
        public Observable<GameSettingsState> LoadSettingsState();

        public Observable<bool> SaveGameState();
        public Observable<bool> SaveSettingsState();

        public Observable<bool> ResetGameState();
        public Observable<GameSettingsState> ResetSettingsState();
    }
}