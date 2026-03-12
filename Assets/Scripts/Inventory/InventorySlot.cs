using System;

namespace PlayerInteraction.Inventory
{
    [Serializable]
    public class InventorySlot
    {
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
