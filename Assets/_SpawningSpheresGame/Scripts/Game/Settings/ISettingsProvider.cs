using System.Threading.Tasks;

namespace SpawningSpheresGame.Game.Settings
{
    public interface ISettingsProvider
    {
        GameSettings GameSettings { get; }
        ApplicationSettings ApplicationSettings { get; }


        Task<GameSettings> LoadGameSettings();
    }
}