using SpawningSpheresGame.Game.GameRoot;

namespace SpawningSpheresGame.Game.MainMenu.Root.MainMenuParams
{
    public class MainMenuExitParams
    {
        public SceneEnterParams TargetSceneEnterParams { get; }


        public MainMenuExitParams(SceneEnterParams targetSceneEnterPasrams)
        {
            TargetSceneEnterParams = targetSceneEnterPasrams;
        }
    }
}