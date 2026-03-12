using System;
using UnityEngine;

namespace PlayerInteraction.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        // Detects changes to inventory
        public InventorySlot[] slots;
        public static Action<ItemSO> OnItemPickup;
        public static Action<ItemSO> OnItemDrop;

        // Tracks possession of items
        // Should make a slot for every collectible artifact
        private InventorySlot _artifact1;
        private InventorySlot _artifact2;
        
        public ItemSO itemSo1;
        public ItemSO itemSo2;
    
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _artifact1 = new InventorySlot(itemSo1);
            _artifact2 = new InventorySlot(itemSo2);
            
            slots = new []{_artifact1, _artifact2};
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
        
        private void PickupItem(ItemSO itemSO)
        {
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot._itemSo != itemSO) continue;
                inventorySlot.foundItem = true;
                inventorySlot.hasItem = true;
            }
        }

        private void DropItem(ItemSO itemSO)
        {
            foreach (var inventorySlot in slots)
            {
                if (inventorySlot._itemSo != itemSO) continue;
                inventorySlot.hasItem = false;
            }
        }
        
    }
}
