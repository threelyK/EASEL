using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemPage : MonoBehaviour
    {
        public int pageNumber;

        [SerializeField] 
        private TMP_Text[] _nameTexts;
        [SerializeField] 
        private Image[] _sprites;
        [SerializeField] 
        private TMP_Text[] _descTexts;

        [SerializeField] 
        private int _indexStart; // Can be used to find the true index from the local index
        [SerializeField, Tooltip("Is inclusive.")]
        private int _indexEnd;
        
        private int _numberOfItems;
        
        // Collections for caching
        private InventoryItem[] _pageItems; // size of this does not reflect the page number -  Holds a reference to the itemStatuses

        // These values shouldn't change after setup
        private ItemSO[] _itemSOs;
        private string[] _names;
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

        private void UpdateAllButtonEvents(bool isEnabled)
        {
            for (var i = 0; i < _sprites.Length; i++)
            {
                if (_itemSOs[i] == null) return;
                var relatedItemSO =  _itemSOs[i];
                switch (isEnabled)
                {
                    case true:
                        _sprites[i].GetComponent<Button>().onClick.AddListener(() => InventoryManager.OnItemDrop?.Invoke(relatedItemSO));
                        break;
                    case false:
                        _sprites[i].GetComponent<Button>().onClick.RemoveAllListeners();
                        break;
                }
            }
        }

        #region Unity Methods

        private void Start()
        {
            
            _numberOfItems = (_indexEnd + 1) - _indexStart;
            
            _pageItems = new InventoryItem[_numberOfItems];
            
            _itemSOs =  new ItemSO[_numberOfItems];
            _names = new string[_numberOfItems];
            
            _inventoryManager = InventoryManager.Instance;
            
            if (_inventoryManager.inventoryItems == null) throw new Exception("InventoryManager.inventoryItems not found");
            
            for (var i = _indexStart; i <= _indexEnd; i++)
            {
                var localIndex = i - _indexStart;
                
                // Setting up caches
                _pageItems[localIndex] = _inventoryManager.inventoryItems[i];
                _itemSOs[localIndex] = _pageItems[localIndex]._itemSo;

                // Setting up visuals
                _nameTexts[localIndex].text = "???";
                _names[localIndex] = _inventoryManager.items[i].displayName;
                _descTexts[localIndex].text = _inventoryManager.items[i].description;
                
                _sprites[localIndex].sprite = _inventoryManager.items[i].icon;
            }

            UpdateAllButtonEvents(true);
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
            UpdateAllButtonEvents(false);
        }

        #endregion
        
        
    }
}
