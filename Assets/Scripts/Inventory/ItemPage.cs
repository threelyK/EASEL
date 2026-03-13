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

        [SerializeField] private int _numberOfPageItems = 3;
        private InventoryItem[] _pageItems;
        private ItemSO[] _itemSOs;

        [SerializeField] private TMP_Text[] _nameTexts;
        [SerializeField] private Image[] _sprites;
        [SerializeField] private TMP_Text[] _descTexts;
        
        [SerializeField] private int _indexStart;
        [SerializeField] private int _indexEnd;
        void Start()
        {
            _itemSOs = new ItemSO[_numberOfPageItems];
            _pageItems = new InventoryItem[_numberOfPageItems];
            
            for (var i = _indexStart; i <= _indexEnd; i++)
            {
                if (InventoryManager.Instance.inventoryItems == null)
                {
                    throw new Exception("InventoryManager.inventoryItems not found");
                }
                _pageItems[i] = InventoryManager.Instance.inventoryItems[i];
                _itemSOs[i] = _pageItems[i]._itemSo;
                
                _nameTexts[i].text = _itemSOs[i].name;
                _sprites[i].sprite = _itemSOs[i].icon;
                _descTexts[i].text = _itemSOs[i].description;
            }
        }

        void CheckForChanges(int page)
        {
            if (pageNumber != page) return;
            Debug.Log($"Has item: {_pageItems[0].hasItem}");
            // Updates caches for _pageItems;
            // If hasItem = false && foundItem = false --> Black silohuette
            // If hasItem = false && foundItem --> Gray silohuette
            // If hasItem && foundItem --> No silohuette
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
