using UnityEngine;

namespace SpawningSpheresGame.Game.Settings.DataStorage
{
    [CreateAssetMenu(fileName = "ConfigsStorage", menuName = "Game Settings/Storage/New ConfigsStorage")]
    public class ConfigsStorage : ScriptableObject
    {
        [SerializeField] private ZoomRtsCameraConfig _zoomRtsCameraConfig;
        [SerializeField] private MoveRtsCameraConfig _moveRtsCameraConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private AdvancedPlayerConfig _advancedPlayerConfig;
        [SerializeField] private CreatureConfig _enemyConfig;
        [SerializeField] private TestConfig _testConfig;

        [SerializeField] private BaseConfig[] _customConfigs;

        public ZoomRtsCameraConfig ZoomRtsCameraConfig => _zoomRtsCameraConfig;
        public MoveRtsCameraConfig MoveRtsCameraConfig => _moveRtsCameraConfig;
        public PlayerConfig PlayerConfig => _playerConfig;
        public AdvancedPlayerConfig AdvancedPlayerConfig => _advancedPlayerConfig;
        public CreatureConfig EnemyConfig => _enemyConfig;
        public TestConfig TestConfig => _testConfig;


        public T GetCustomConfig<T>(string configName = null) where T : BaseConfig
        {
            foreach (var config in _customConfigs)
            {
                if (config is T typedConfig && (configName == null || config.ConfigName == configName))
                {
                    return typedConfig;
                }
            }
            return null;
        }
    }
}