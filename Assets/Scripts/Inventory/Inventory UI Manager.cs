using System;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryUIManager : Singleton<InventoryUIManager>
    {
        // Pages are manually made
        // TODO: first page has 3 artifacts

        [SerializeField] private GameObject _mainPage;
        public ItemPage[] _pages;

        [SerializeField] private TMP_Text _pageNumberText;
        
        [SerializeField] private int pageCount; // 2 pages = index 0,1
        private int pageNumber; // First page = 0
        private int prevPageNumber;

        public static Action<int> OnPageChanged;
        
        public void HandlePageChange(int forwardAmount)
        {
            switch (forwardAmount)
            {
                case > 0:
                    if (pageNumber + forwardAmount <= pageCount)
                    {
                        prevPageNumber = pageNumber;
                        pageNumber += forwardAmount;
                        DisplayPage(pageNumber);
                        HidePage(prevPageNumber);
                    }
                    break;
                case < 0:
                    if (pageNumber + forwardAmount >= 0)
                    {
                        prevPageNumber = pageNumber;
                        pageNumber += forwardAmount;
                        DisplayPage(pageNumber);
                        HidePage(prevPageNumber);
                    }
                    break;
                default:
                    return;
                OnPageChanged?.Invoke(pageNumber);
            }
        }

        private void DisplayPage(int pageNum)
        {
            var page =  _pages[pageNum];
            _pageNumberText.text = $"Page: {pageNum}";
            page.gameObject.SetActive(true);
        }

        private void HidePage(int pageNum)
        {
            var page = _pages[pageNum];
            page.gameObject.SetActive(false);
        }

        #region  Unity Methods

        private void Start()
        {
            _pageNumberText.text = $"Page: {pageNumber+1}";
        }

        #endregion
        
    }
}
