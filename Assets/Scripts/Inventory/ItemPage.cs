using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public enum ItemStatus
    {
        HELD,
        FOUND,
        NOT_FOUND,
    }
    public class ItemPage : MonoBehaviour
    {
        // Change Inventory Item shown on Inventory Display Object
        // Format:
        // Top --> Name -- if not found name = ???
        // Middle --> Object Preview
        // Bot --> Description
        
        public int pageNumber;

        [SerializeField] private TMP_Text[] _nameTexts;
        [SerializeField] private Image[] _sprites;
        [SerializeField] private TMP_Text[] _descTexts;
        
        [SerializeField] private int _numberOfPageItems = 3;
        private InventoryItem[] _pageItems; // size of this does not reflect the page number

        private string[] _names;
        
        [SerializeField] private int _indexStart; // Can be used to find the true index from the local index
        [SerializeField] private int _indexEnd;
        
        private InventoryManager _inventoryManager;


        void CheckForChanges(int pageNum)
        {
            // TODO: figure out masking of the image
            if (pageNumber != pageNum) return;
            
            // Delete these after test
            Debug.Log($"Has item: {_pageItems[0].hasItem}");
            Debug.Log($"Has item invm: {_inventoryManager.inventoryItems[0].hasItem}");
            
            // Updates caches for _pageItems;
            // If hasItem = false && foundItem = false --> Black silhouette
            // If hasItem = false && foundItem --> Gray silhouette
            // If hasItem && foundItem --> No silhouette
        }
        
        private void Start()
        {
            _pageItems = new InventoryItem[_numberOfPageItems];
            _names = new string[_numberOfPageItems];
            
            _inventoryManager = InventoryManager.Instance;
            
            if (_inventoryManager.inventoryItems == null) throw new Exception("InventoryManager.inventoryItems not found");
            
            for (var i = _indexStart; i <= _indexEnd; i++)
            {
                var localIndex = i - _indexStart;
                _pageItems[localIndex] = _inventoryManager.inventoryItems[i];
                
                _nameTexts[localIndex].text = "???";
                _names[localIndex] = _inventoryManager.items[i].displayName;
                _sprites[localIndex].sprite = _inventoryManager.items[i].icon;
                _descTexts[localIndex].text = _inventoryManager.items[i].description;
            }
        }

        private void OnEnable()
        {
            InventoryUIManager.OnPageChanged += CheckForChanges;
        }
        
        private void OnDisable()
        {
            InventoryUIManager.OnPageChanged -= CheckForChanges;
        }
    }
}
