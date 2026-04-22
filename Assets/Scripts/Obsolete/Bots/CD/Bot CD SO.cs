using UnityEngine;

namespace Bots.CD
{
    [CreateAssetMenu(fileName = "BotCDSO", menuName = "Scriptable Objects/Bots/Bot CD")]
    public class BotCDSO : ScriptableObject
    {
        public BotIntent intent;
        public GameObject CDPrefab;
    }
}
