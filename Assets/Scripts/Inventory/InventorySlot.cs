using System;

namespace Inventory
{
    [Serializable]
    public class InventorySlot
    {
        // GUI related inventory
        
        
        public bool foundItem;
        // _hasItem was switched on then display grayed-out image ("in world") instead of black

        public bool hasItem; // True when Item is dropped in inventory

        public ItemSO _itemSo;

        public InventorySlot(ItemSO item)
        {
            _itemSo = item;
        }
    }
}
