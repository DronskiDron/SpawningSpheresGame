using System.Collections.Generic;
using Newtonsoft.Json;
using SpawningSpheresGame.Game.State.DataTypes;
using UnityEngine;

namespace SpawningSpheresGame.JsonSerializationExample
{
    public class JsonSerializationExample : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;

        private const string STATE = nameof(STATE);
        private GameStateJsonTest _gameState;
        private CharacterEntityJsonTest _character;


        private void Start()
        {
            JsonProjectSettingsTest.ApplyProjectSerializationSettings();

            _gameState = LoadState();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateRandomCharacter();
                return;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                CreateRandomDoor();
                return;
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                CreateRandomPickubleItem();
                return;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                InstantiateTestPrefab();
                return;
            }
        }


        private GameStateJsonTest LoadState()
        {
            var stateJson = PlayerPrefs.GetString(STATE);
            Debug.Log("Loaded state: " + stateJson);

            var state = string.IsNullOrEmpty(stateJson)
            ? new GameStateJsonTest { Entities = new List<EntityJsonTest>() }
            : JsonConvert.DeserializeObject<GameStateJsonTest>(stateJson);

            Debug.Log("Fully loaded state " + JsonConvert.SerializeObject(state));

            return state;
        }


        private void SaveState()
        {
            var stateJson = JsonConvert.SerializeObject(_gameState);
            Debug.Log("State to save: " + stateJson);

            PlayerPrefs.SetString(STATE, stateJson);
        }


        private void CreateRandomCharacter()
        {
            var characterEntity = new CharacterEntityJsonTest
            {
                Id = _gameState.GetNewId(),
                Type = EntityTypeJsonTest.Character,
                // Position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                // Rotation = new Quaternion(Random.Range(-10, 10), 0, Random.Range(-10, 10), 0),
                Position = new Vector3(0, 30, 0),
                Rotation = Quaternion.identity,
                Level = Random.Range(0, 21),
                TransformStateData = new TransformStateData
                {
                    Position = new Vector3(1, 60, 0)
                }
            };

            _gameState.Entities.Add(characterEntity);
            characterEntity.Position = characterEntity.TransformStateData.Position;
            _character = characterEntity;

            SaveState();
        }


        private void CreateRandomDoor()
        {
            var doorEntity = new DoorEntityJsonTest
            {
                Id = _gameState.GetNewId(),
                Type = EntityTypeJsonTest.Door,
                Position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                Rotation = new Quaternion(Random.Range(-10, 10), 0, Random.Range(-10, 10), 0),
                IsLocked = Random.Range(0, 2) == 0,
                IsOpen = Random.Range(0, 2) == 0
            };

            _gameState.Entities.Add(doorEntity);

            SaveState();
        }


        private static string[] ItemIds = { "Apple", "Orange", "Banana", "Pineapple" };


        private void CreateRandomPickubleItem()
        {
            var pickubleItemEntity = new PickubleItemEntityJsonTest
            {
                Id = _gameState.GetNewId(),
                Type = EntityTypeJsonTest.PickubleItem,
                Position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)),
                Rotation = new Quaternion(Random.Range(-10, 10), 0, Random.Range(-10, 10), 0),
                ItemId = ItemIds[Random.Range(0, ItemIds.Length)],
                Amount = Random.Range(0, 100)
            };

            _gameState.Entities.Add(pickubleItemEntity);

            SaveState();
        }


        private void InstantiateTestPrefab()
        {
            GameObject.Instantiate(_prefab, _character.Position, _character.Rotation);
        }
    }
}
