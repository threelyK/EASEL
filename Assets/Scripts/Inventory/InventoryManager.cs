using System;
using System.Linq;
using UnityEngine.Serialization;

namespace Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // Doesn't handle UI elements only data
        
        // Detects changes to inventory
        public static Action<ItemSO> OnItemPickup;
        public static Action<ItemSO> OnItemDrop;

        // Tracks possession of items
        private const int _itemCount = 3; // Total number of artifacts
        private InventoryItem[] _inventoryItems = new InventoryItem[_itemCount];
        public ItemSO[] items = new ItemSO[_itemCount];
        
        private void PickupItem(ItemSO item)
        {
            _inventoryItems.First(inventoryItem => inventoryItem._itemSo.id == item.id).ItemPickedUp(); 
        }
        
        private void DropItem(ItemSO item)
        {
            _inventoryItems.First(inventoryItem => inventoryItem._itemSo.id == item.id).ItemDropped(); 
        }
        
        private void Start()
        {
            // Initializing inventory
            for (var i = 0; i < _inventoryItems.Length; i++)
            {
                _inventoryItems[i] = new InventoryItem(items[i]);
            }

        }
        
        private void OnEnable()
        {
            OnItemPickup += PickupItem;
            OnItemDrop += DropItem;
        }

        private void OnDisable()
        {
            OnItemPickup -= PickupItem;
            OnItemDrop -= DropItem;
        }
    }
}
