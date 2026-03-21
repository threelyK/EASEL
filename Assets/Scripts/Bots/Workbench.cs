
using System;
using Bots.Components;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bots
{
    public class Workbench : MonoBehaviour
    {
        // Handles the placing, repairing of broken bots, and spawning of fixed robots
        public bool isWorkbenchBusy;
        [SerializeField] private Transform _snapLocation;
        
        private GameObject _brokenBot;

        [SerializeField] private GameObject[] _botPrefabs;
        
        private void Start()
        {
            if (_snapLocation == null) Debug.LogError("Snap location is null");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Bot")) return;
            if (isWorkbenchBusy) return;
                
            _brokenBot = other.gameObject;
            Debug.LogWarning("BrokenBot: " + _brokenBot.name);

            if (!_brokenBot.TryGetComponent<WIPBot>(out var wipBot)) return;
            Debug.LogWarning("WIP bot component found");
                
            // Setup workbench for bot
            wipBot.isOnWorkbench = true;
            isWorkbenchBusy = true;
                    
            // Snap bot into place
            
            if (_brokenBot.TryGetComponent<BGBotController>(out var botController))
            {
                botController.MoveToSnap(_snapLocation);
                Debug.LogWarning("Snapped robot onto table");
            }
                    
            // Disable poke interactable on head
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Bot")) return;
            var obj = other.gameObject;
            if (obj == _brokenBot)
            {
                ResetBench();
            }
        }

        private void OnEnable()
        {
            WIPBot.OnBotRepaired += RepairBot;
        }

        private void OnDisable()
        {
            WIPBot.OnBotRepaired -= RepairBot;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellowNice;
            Gizmos.DrawWireCube(_snapLocation.position, new Vector3(0.5f, 0.5f, 0.5f));
        }

        private void ResetBench()
        {
            _brokenBot = null;
            isWorkbenchBusy = false;
        }

        private void RepairBot(int botID)
        {
            if (botID == _brokenBot.GetInstanceID())
            {
                SpawnRobot();
                ResetBench();
            }
        }

        private void SpawnRobot()
        {
            var botModel = _botPrefabs[Random.Range(0, _botPrefabs.Length)];
            
            // TODO check rotation is correct
            Instantiate(botModel, _snapLocation.position, _snapLocation.rotation);
        }
        
    }
}
