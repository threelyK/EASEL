using System;
using TMPro;
using Unity.VisualScripting;
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
        public int pageNumber;

        [SerializeField] private TMP_Text[] _nameTexts;
        [SerializeField] private Image[] _sprites;
        [SerializeField] private TMP_Text[] _descTexts;
        
        [SerializeField] private int _numberOfPageItems = 3;
        private InventoryItem[] _pageItems; // size of this does not reflect the page number

        private ItemStatus[] _pageItemStatuses;
        private string[] _names;
        
        [SerializeField] private int _indexStart; // Can be used to find the true index from the local index
        [SerializeField] private int _indexEnd;
        
        private InventoryManager _inventoryManager;


        void CheckForChanges(int pageNum)
        {
            // TODO: figure out masking of the image
            if (pageNumber != pageNum) return;
            
            // Check all pageItems for updates
            for (var i = 0; i < _pageItems.Length; i++)
            {
                if (_pageItems[i].HasItem())
                {
                    _pageItemStatuses[i] = ItemStatus.HELD;
                } else if (_pageItems[i].FoundItem())
                {
                    _pageItemStatuses[i] = ItemStatus.FOUND;
                }
                else
                {
                    _pageItemStatuses[i] = ItemStatus.NOT_FOUND;
                }
            }

            UpdateCurrentPage();
        }

        private void UpdateCurrentPage()
        {
            for (int i = 0; i < _pageItemStatuses.Length; i++)
            {
                var status = _pageItemStatuses[i];
                switch (status)
                {
                    case ItemStatus.HELD:
                        _nameTexts[i].text = _names[i];
                        _sprites[i].color = Color.white;
                        break;
                    case ItemStatus.FOUND:
                        // Change silhouette
                        _sprites[i].color = new Color32(0, 0, 0, 100);
                        break;
                    case ItemStatus.NOT_FOUND:
                        // Change silhouette
                        _sprites[i].color = Color.black;
                        break;
                }
            }
        }
        
        private void Start()
        {
            _pageItems = new InventoryItem[_numberOfPageItems];
            _names = new string[_numberOfPageItems];
            _pageItemStatuses = new ItemStatus[_numberOfPageItems];
            
            _inventoryManager = InventoryManager.Instance;
            
            if (_inventoryManager.inventoryItems == null) throw new Exception("InventoryManager.inventoryItems not found");
            
            for (var i = _indexStart; i <= _indexEnd; i++)
            {
                var localIndex = i - _indexStart;
                _pageItems[localIndex] = _inventoryManager.inventoryItems[i];


                _pageItemStatuses[localIndex] = ItemStatus.NOT_FOUND;
                _nameTexts[localIndex].text = "???";
                _names[localIndex] = _inventoryManager.items[i].displayName;
                _sprites[localIndex].sprite = _inventoryManager.items[i].icon;
                _descTexts[localIndex].text = _inventoryManager.items[i].description;
            }
            
            CheckForChanges(pageNumber);
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
