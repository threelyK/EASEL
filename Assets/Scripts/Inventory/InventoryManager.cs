using System;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // Doesn't handle UI elements only data
        
        // Detects changes to inventory
        public static Action<ItemSO> OnItemPickup;
        public static Action<ItemSO> OnItemDrop;

        // Tracks possession of items
        public InventoryItem[] inventoryItems; // TODO Do I need to expose this?
        public ItemSO[] items;
        
        [SerializeField] private GameObject _SpawnPosObj;
        
        private void PickupItem(ItemSO item)
        {
            var foundItem = Array.Find(inventoryItems, inventoryItem => inventoryItem._itemSo == item);
            
            if  (foundItem == null) throw new Exception("Can't find item in inventoryItems");
            foundItem?.ItemPickedUp();
        }

        private void DropItem(ItemSO item)
        {
            var foundItem = Array.Find(inventoryItems, invItem =>
                invItem._itemSo == item && invItem.GetItemStatus() == ItemStatus.HELD);

            if (foundItem == null) throw new Exception("Can't find item in inventoryItems");
            Debug.Log("Dropping item");
            
            Instantiate(foundItem._itemSo.worldPrefab, _SpawnPosObj.transform.position, _SpawnPosObj.transform.rotation);
            foundItem?.ItemDropped();
        }
        
        private void Start()
        {
            inventoryItems = new InventoryItem[items.Length];
            
            // Initializing inventory
            for (var i = 0; i < items.Length; i++)
            {
                // Making an inventoryItem for every item that can be put into the inventory
                inventoryItems[i] = new InventoryItem(items[i]);
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
