using UnityEngine;

namespace Inventory
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] 
        private ItemSO _itemSO;
        
        // Detects when in inventory zone
        private void OnTriggerStay(Collider other)
        {
            if (!other.GetComponent<Collider>().CompareTag("InventoryDropZone")) return;
            PlaceIntoInventory();
        }

        private void PlaceIntoInventory()
        {
            InventoryManager.OnItemPickup?.Invoke(_itemSO);
            // Play some audio
            Destroy(gameObject);
        }
    }
}
