using System;

namespace Inventory
{
    public class InventoryUIManager : Singleton<InventoryUIManager>
    {
        // Pages are manually made
        // TODO: first page has 3 artifacts
        
        private int pageNumber; // First page = 0
        private int maxPageNumber;

        public static Action<int> OnPageForwardChange;
        
        private void OnEnable()
        {
            OnPageForwardChange += HandlePageChange;
        }

        private void HandlePageChange(int forwardAmount)
        {
            switch (forwardAmount)
            {
                case > 0:
                    if (pageNumber + forwardAmount <= maxPageNumber) pageNumber += forwardAmount;
                    break;
                case < 0:
                    if (pageNumber + forwardAmount >= 0) pageNumber += forwardAmount;
                    break;
            }
        }

        private void DisplayPage(int pageNumber)
        {
            // Change Inventory Item shown on Inventory Display Object
            // Format:
            // Top --> Name -- if not found name = ???
            // Middle --> Object Preview
            // Bot --> Description
            
            
        }
        
        
    }
}
