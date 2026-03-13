using UnityEngine;

namespace Inventory
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private ItemSO _itemSO;
        
        // Detects when in inventory zone
        private void OnTriggerStay(Collider other)
        {
            if (!other.GetComponent<Collider>().CompareTag("InventoryDropZone")) return;
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) ||
                OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                PlaceIntoInventory();
            }
        }

        private void PlaceIntoInventory()
        {
            InventoryManager.OnItemPickup?.Invoke(_itemSO);
            Debug.Log("Placed into inventory");
            Destroy(gameObject);
        }
    }
}
