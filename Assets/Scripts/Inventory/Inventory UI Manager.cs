using System;
using TMPro;
using UnityEngine;

namespace Inventory
{
    public class InventoryUIManager : Singleton<InventoryUIManager>
    {
        // Pages are manually made
        // TODO: first page has 3 artifacts

        [SerializeField] private GameObject _mainPage; // Used to show/hide whenever the inventory is brought up
        public ItemPage[] _pages;

        [SerializeField] private TMP_Text _pageNumberText;
        
        private int _pageCount; // 2 pages = index 0,1
        private int pageIndex; // First page = 0
        private int prevPageIndex;

        public static Action<int> OnPageChanged;
        
        public void HandlePageChange(int forwardAmount)
        {
            switch (forwardAmount)
            {
                case > 0:
                    if (pageIndex + forwardAmount < _pageCount)
                    {
                        prevPageIndex = pageIndex;
                        pageIndex += forwardAmount;
                        DisplayPage(pageIndex);
                        HidePage(prevPageIndex);
                    }
                    break;
                case < 0:
                    if (pageIndex + forwardAmount >= 0)
                    {
                        prevPageIndex = pageIndex;
                        pageIndex += forwardAmount;
                        DisplayPage(pageIndex);
                        HidePage(prevPageIndex);
                    }
                    break;
                default:
                    return;
            }

            OnPageChanged?.Invoke(pageIndex);
        }

        private void DisplayPage(int pageNum)
        {
            var page =  _pages[pageNum];
            _pageNumberText.text = $"Page: {pageIndex+1}";
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
            _pageCount = _pages.Length;
            _pageNumberText.text = $"Page: {pageIndex+1}";
            for (var i = 1; i < _pageCount; i++)
            {
                _pages[i].gameObject.SetActive(false);
            }
        }

        #endregion
        
    }
}
