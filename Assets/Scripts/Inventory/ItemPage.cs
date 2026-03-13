using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemPage : MonoBehaviour
    {
        public int pageNumber;

        [SerializeField] private TMP_Text[] _nameTexts;
        [SerializeField] private Image[] _sprites;
        [SerializeField] private TMP_Text[] _descTexts;
        
        [SerializeField] private int _numberOfPageItems = 3;
        private InventoryItem[] _pageItems; // size of this does not reflect the page number
        private ItemSO[] _itemSOs;
        
        private string[] _names;
        
        [SerializeField] private int _indexStart; // Can be used to find the true index from the local index
        [SerializeField] private int _indexEnd;
        
        private InventoryManager _inventoryManager;

        private void UpdatePage(int pageNum)
        {
            if (pageNum != pageNumber) return;
            UpdateCurrentPage();
        }

        private void UpdatePageWithItem(ItemSO itemSO)
        {
            if (_itemSOs.Contains(itemSO))
            {
                UpdateCurrentPage();
            }
        }
        
        private void UpdateCurrentPage()
        {
            for (var i = 0; i < _pageItems.Length; i++)
            {
                var status = _pageItems[i].GetItemStatus();
                switch (status)
                {
                    case ItemStatus.HELD:
                        _nameTexts[i].text = _names[i];
                        _sprites[i].color = Color.white;
                        break;
                    case ItemStatus.FOUND:
                        _sprites[i].color = new Color32(0, 0, 0, 100);
                        break;
                    case ItemStatus.NOT_FOUND:
                        _sprites[i].color = Color.black;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private void Start()
        {
            _pageItems = new InventoryItem[_numberOfPageItems];
            _names = new string[_numberOfPageItems];
            
            _itemSOs =  new ItemSO[_numberOfPageItems];
            
            _inventoryManager = InventoryManager.Instance;
            
            if (_inventoryManager.inventoryItems == null) throw new Exception("InventoryManager.inventoryItems not found");
            
            for (var i = _indexStart; i <= _indexEnd; i++)
            {
                var localIndex = i - _indexStart;
                _pageItems[localIndex] = _inventoryManager.inventoryItems[i];
                _itemSOs[localIndex] = _pageItems[localIndex]._itemSo;

                _nameTexts[localIndex].text = "???";
                _names[localIndex] = _inventoryManager.items[i].displayName;
                _sprites[localIndex].sprite = _inventoryManager.items[i].icon;
                _descTexts[localIndex].text = _inventoryManager.items[i].description;
            }

            UpdateCurrentPage();
        }

        private void OnEnable()
        {
            InventoryUIManager.OnPageChanged += UpdatePage;
            InventoryManager.OnItemPickup += UpdatePageWithItem;
            InventoryManager.OnItemDrop += UpdatePageWithItem;
        }
        
        private void OnDisable()
        {
            InventoryUIManager.OnPageChanged -= UpdatePage;
            InventoryManager.OnItemPickup -= UpdatePageWithItem;
            InventoryManager.OnItemDrop -= UpdatePageWithItem;
        }
    }
}
