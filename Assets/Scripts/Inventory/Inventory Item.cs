namespace Inventory
{
    public enum ItemStatus
    {
        HELD,
        FOUND,
        NOT_FOUND,
    }
    public class InventoryItem
    {
        private ItemStatus _itemStatus = ItemStatus.NOT_FOUND;
        public ItemSO _itemSo;

        public void ItemPickedUp()
        {
            _itemStatus = ItemStatus.HELD;
        }

        public void ItemDropped()
        {
            _itemStatus = ItemStatus.FOUND;
        }
        
        public InventoryItem(ItemSO itemSo)
        {
            _itemSo = itemSo;
        }

        public ItemStatus GetItemStatus()
        {
            return _itemStatus;
        }
    }
}
