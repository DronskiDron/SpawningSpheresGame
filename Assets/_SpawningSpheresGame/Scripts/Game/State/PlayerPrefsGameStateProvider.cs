using System.Collections.Generic;
using Newtonsoft.Json;
using R3;
using SpawningSpheresGame.Game.State.Entities;
using SpawningSpheresGame.Game.State.Entities.Creatures;
using SpawningSpheresGame.Game.State.Root;
using UnityEngine;

namespace SpawningSpheresGame.Game.State
{
    public class PlayerPrefsGameStateProvider : IGameStateProvider
    {
        private const string GAME_STATE_KEY = nameof(GAME_STATE_KEY);
        private const string GAME_SETTINGS_STATE_KEY = nameof(GAME_SETTINGS_STATE_KEY);
        private readonly Subject<Unit> _preSaveSubj = new Subject<Unit>();

        public GameState GameState { get; private set; }
        public GameSettingsState SettingsState { get; private set; }
        public Observable<Unit> OnPreSave { get => _preSaveSubj; }

        private GameStateData _gameStateOrigin;
        private GameSettingsStateData _gameSettingsStateOrigin;


        public Observable<GameState> LoadGameState()
        {
            if (!PlayerPrefs.HasKey(GAME_STATE_KEY))
            {
                GameState = CreateGameStateFromSettings();
                SaveGameState();
            }
            else
            {
                var json = PlayerPrefs.GetString(GAME_STATE_KEY);

                _gameStateOrigin = JsonConvert.DeserializeObject<GameStateData>(json);
                GameState = new GameState(_gameStateOrigin);

                Debug.Log("Game state loaded: " + json);
                // Debug.Log("Fully loaded state " + JsonConvert.SerializeObject(_gameStateOrigin));
            }

            return Observable.Return(GameState);
        }


        public Observable<GameSettingsState> LoadSettingsState()
        {
            if (!PlayerPrefs.HasKey(GAME_SETTINGS_STATE_KEY))
            {
                SettingsState = CreateGameSettingsStateFromSettings();

                SaveSettingsState();
            }
            else
            {
                var json = PlayerPrefs.GetString(GAME_SETTINGS_STATE_KEY);
                _gameSettingsStateOrigin = JsonUtility.FromJson<GameSettingsStateData>(json);
                SettingsState = new GameSettingsState(_gameSettingsStateOrigin);
            }

            return Observable.Return(SettingsState);
        }


        public Observable<bool> ResetGameState()
        {
            GameState = CreateGameStateFromSettings();
            SaveGameState();

            return Observable.Return(true);
        }


        public Observable<GameSettingsState> ResetSettingsState()
        {
            SettingsState = CreateGameSettingsStateFromSettings();
            SaveSettingsState();

            return Observable.Return(SettingsState);
        }


        public Observable<bool> SaveGameState()
        {
            _preSaveSubj.OnNext(Unit.Default);
            var json = JsonConvert.SerializeObject(_gameStateOrigin);
            Debug.Log("I try Save that data: " + json);
            PlayerPrefs.SetString(GAME_STATE_KEY, json);

            return Observable.Return(true);
        }


        public Observable<bool> SaveSettingsState()
        {
            var json = JsonUtility.ToJson(_gameSettingsStateOrigin, true);
            PlayerPrefs.SetString(GAME_SETTINGS_STATE_KEY, json);

            return Observable.Return(true);
        }


        private GameState CreateGameStateFromSettings()
        {
            _gameStateOrigin = new GameStateData
            {
                Creatures = new List<CreatureEntityData>(),
                CustomEntities = new List<EntityData>(),
            };

            return new GameState(_gameStateOrigin);
        }


        private GameSettingsState CreateGameSettingsStateFromSettings()
        {
            _gameSettingsStateOrigin = new GameSettingsStateData
            {
                MusicVolume = 5,
                SFXVolume = 5
            };

            return new GameSettingsState(_gameSettingsStateOrigin);
        }
    }
}