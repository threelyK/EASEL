using UnityEngine;

namespace PlayerInteraction.Inventory
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/Inventory System/Item")]
    public class ItemSO : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite icon; // Can be changed to a 3D preview prefab
        public GameObject worldPrefab; // Obj spawned when placed
        public string description;
    }
}
