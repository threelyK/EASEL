namespace Inventory
{
    public class InventoryItem
    {
        private bool _foundItem;
        private bool _hasItem;
        public ItemSO _itemSo;

        public void ItemPickedUp()
        {
            if (!_foundItem) _foundItem = true;
            _hasItem = true;
        }

        public void ItemDropped()
        {
            _hasItem = false;
        }
        
        public InventoryItem(ItemSO itemSo)
        {
            _itemSo = itemSo;
        }

        public bool HasItem()
        {
            return _hasItem;
        }

        public bool FoundItem()
        {
            return _foundItem;
        }
    }
}
