using System;
using PlayerInteraction.Inventory;
using UnityEngine;

namespace Inventory
{
    public class Collectable : MonoBehaviour
    {
        public ItemSO itemSO;

        private void OnTriggerStay(Collider other)
        {
            if (!other.GetComponent<Collider>().CompareTag("InventoryDropZone")) return;
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) ||
                OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            {
                PlaceIntoInventory();
            };
        }

        private void PlaceIntoInventory()
        {
            InventoryManager.OnItemPickup?.Invoke(itemSO);
            Debug.Log("Placed into inventory");
            Destroy(gameObject);
        }

        // TODO:
        // - When in range produce outline to indicate it's interactable
    
    
    }
}
