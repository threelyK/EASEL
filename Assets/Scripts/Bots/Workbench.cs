
using System;
using DG.Tweening;
using UnityEngine;

namespace Bots
{
    public class Workbench : MonoBehaviour
    {
        // Handles the placing, repairing of broken bots, and spawning of fixed robots
        public bool isWorkbenchBusy;
        [SerializeField] private Vector3 _snapLocation;
        
        private GameObject _brokenBot;

        public static Action OnRobotRepaired;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Bot"))
            {
                if (isWorkbenchBusy) return;
                
                // Setup workbench for bot
                isWorkbenchBusy = true;
                
                // Snap bot into place
                var botController = _brokenBot.GetComponent<BGBotController>();
                if (botController == null)
                {
                    Debug.LogError("bot has no controller");
                    ResetBench();
                }
                
                botController.MoveToSnap(_snapLocation);
                
                // Allowing player to insert items into bot
                // Disable poke interactable on head
                
                
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellowNice;
            Gizmos.DrawWireCube(_snapLocation, Vector3.one);
        }

        private void ResetBench()
        {
            _brokenBot = null;
            isWorkbenchBusy = false;
        }
        
    }
}
