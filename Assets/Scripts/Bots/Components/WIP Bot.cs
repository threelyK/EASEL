using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bots.Components
{
    public class WIPBot : MonoBehaviour
    {
        
        [SerializeField] private MissingPartType _missingOnePart = MissingPartType.All;

        private Dictionary<MissingPartType, bool> _partConditions;
        
        public bool isOnWorkbench;
        
        public static Action<int> OnBotRepaired;

        private void Start()
        {
            _partConditions = new Dictionary<MissingPartType, bool>
            {
                { MissingPartType.Perception, false },
                { MissingPartType.Reasoning, false },
                { MissingPartType.Tools, false },
                { MissingPartType.Memory, false },
                { MissingPartType.Learning, false }
            };
            if (_missingOnePart == MissingPartType.All) return;

            foreach (var key in _partConditions.Keys.ToList().Where(key => key != _missingOnePart))
            {
                _partConditions[key] = true;
            }
        }

        public void OnPartAdded(MissingPartType part)
        {
            _partConditions[part] = true;

            if (!IsComplete()) return;
            
            // Do things if complete
            OnBotRepaired?.Invoke(gameObject.GetInstanceID());
            Destroy(gameObject);
        }

        public bool hasPart(MissingPartType part)
        {
            return _partConditions[part];
        }

        private bool IsComplete()
        {
            return !_partConditions.ContainsValue(false);
        }
    }
}
