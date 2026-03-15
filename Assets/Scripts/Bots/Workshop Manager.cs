using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bots
{
    public class WorkshopManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnLocation;
        
        private int _colours;
        [SerializeField] private GameObject[] _botPrefabs;

        private void HandleBotCompletion()
        {
            var botModel = _botPrefabs[Random.Range(0, _botPrefabs.Length)];
            
            // Instantiate bot
            Instantiate(botModel, _spawnLocation.position, _spawnLocation.rotation);
            // TODO: Play sound effect + particles
        }

        // Used whenever we want to change the bot spawn location
        public void UpdateBotSpawnLocation(Vector3 newPosition, Quaternion newRotation)
        {
            _spawnLocation.position = newPosition;
            _spawnLocation.rotation = newRotation;
        }

        private void OnEnable()
        {
            WIPBot.OnCompleteBot += HandleBotCompletion;
        }

        private void OnDisable()
        {
            WIPBot.OnCompleteBot -= HandleBotCompletion;
        }
    }
}
