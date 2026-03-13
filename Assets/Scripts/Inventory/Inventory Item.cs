

using System;

namespace Inventory
{
    public class InventoryItem
    {
        private bool _foundItem;
        public bool hasItem;
        public ItemSO _itemSo;

        public void ItemPickedUp()
        {
            if (!_foundItem) _foundItem = true;
            hasItem = true;
        }

        public void ItemDropped()
        {
            hasItem = false;
        }
        
        public InventoryItem(ItemSO itemSo)
        {
            _itemSo = itemSo;
        }
    }
}
