using System;
using System.Linq;
using UnityEngine;

namespace Bots
{
    internal class PartCondition
    {
        public readonly BotPartType _partType;
        public bool _condition;

        public PartCondition(BotPartType partType, bool condition)
        {
            _partType = partType;
            _condition = condition;
        }
    }
    public class WIPBot : MonoBehaviour
    {
        // Has collider that interacts with triggers on component objects
        
        private PartCondition[] _partConditions = 
        {
            new (BotPartType.PERCEPTION, false),
            new (BotPartType.REASONING, false),
            new (BotPartType.TOOLS, false),
            new (BotPartType.MEMORY, false),
            new (BotPartType.LEARNING, false)
        };
        
        public static Action OnCompleteBot;
        
        
        public void OnPartAdded(BotPartType part)
        {
            var targetPart = Array.Find(_partConditions, partCondition => partCondition._partType == part);
            targetPart._condition = true;

            if (!IsComplete()) return;
            
            // Do things if complete
            OnCompleteBot?.Invoke();
            Destroy(gameObject);
        }

        public bool hasPart(BotPartType part)
        {
            return Array.Find(_partConditions, partCondition => partCondition._partType == part)._condition;
        }

        private bool IsComplete()
        {
            return _partConditions.All(partCondition => partCondition._condition);
        }
    }
}
