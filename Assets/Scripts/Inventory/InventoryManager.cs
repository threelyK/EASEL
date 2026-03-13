using System;
using System.Linq;

namespace Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        // Doesn't handle UI elements only data
        
        // Detects changes to inventory
        public static Action<ItemSO> OnItemPickup;
        public static Action<ItemSO> OnItemDrop;

        // Tracks possession of items
        public InventoryItem[] inventoryItems;
        public ItemSO[] items;
        
        private void PickupItem(ItemSO item)
        {
            inventoryItems.First(inventoryItem => inventoryItem._itemSo.id == item.id).ItemPickedUp(); 
        }
        
        private void DropItem(ItemSO item)
        {
            inventoryItems.First(inventoryItem => inventoryItem._itemSo.id == item.id).ItemDropped(); 
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
