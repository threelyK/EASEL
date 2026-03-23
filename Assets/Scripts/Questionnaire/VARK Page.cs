using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Questionnaire
{
    public class VARKPage: Singleton<VARKPage>
    {
        [SerializeField] private VARKSO _questionnaire;
        [SerializeField] private GameObject _infoPage;
        [SerializeField] private GameObject _mainPage;
        [SerializeField] private GameObject _scorePage;
    
        private string[] _questions;
        private string[] _visual;
        private string[] _aural;
        private string[] _rw;
        private string[] _kines;

        private int _vScore;
        private int _aScore;
        private int _rScore;
        private int _kScore;

        private int _currentPageNumber;
        private int _lastPageNumber;
    
        [SerializeField] private Toggle _visToggle;
        [SerializeField] private Toggle _aurToggle;
        [SerializeField] private Toggle _rwToggle;
        [SerializeField] private Toggle _kinToggle;
    
        [SerializeField] private TMP_Text _questionText;
        [SerializeField] private TMP_Text _visualText;
        [SerializeField] private TMP_Text _auralText;
        [SerializeField] private TMP_Text _rwText;
        [SerializeField] private TMP_Text _kinesText;
    
        private void Start()
        {
            var sections = _questionnaire.VARK;
            _lastPageNumber = sections.Length - 1;

            _questions = new string[sections.Length];
            _visual = new string[sections.Length];
            _aural = new string[sections.Length];
            _rw = new string[sections.Length];
            _kines = new string[sections.Length];
        
            for (var i = 0; i < sections.Length; i++)
            {
                _questions[i] = sections[i].question;
                _visual[i] = sections[i].optionA;
                _aural[i] = sections[i].optionB;
                _rw[i] = sections[i].optionC;
                _kines[i] = sections[i].optionD;
            }
            
            UpdatePage(0);
            _infoPage.SetActive(true);
            _mainPage.SetActive(false);
        }

        private void UpdatePage(int pageNumber)
        {
        
            _questionText.text = _questions[pageNumber];
            _visualText.text = _visual[pageNumber];
            _auralText.text = _aural[pageNumber];
            _rwText.text = _rw[pageNumber];
            _kinesText.text = _kines[pageNumber];
        }

        public void CloseInfo()
        {
            Destroy(_infoPage);
            _mainPage.SetActive(true);
        }

        public void ConfirmSelection()
        {
            if (_currentPageNumber != _lastPageNumber)
            {
                // Changing current index
                _currentPageNumber++;
                ProcessSelection();
                UpdatePage(_currentPageNumber);
            }

            if (_currentPageNumber == _lastPageNumber)
            {
                ProcessSelection();
                UpdateSessionScores();
                Destroy(gameObject);
            }
        }

        private void ProcessSelection()
        {
            if (_visToggle.isOn)
            {
                _vScore++;
                _visToggle.isOn = false;
            }

            if (_aurToggle.isOn)
            {
                _aScore++;
                _aurToggle.isOn = false;
            }

            if (_rwToggle.isOn)
            {
                _rScore++;
                _rwToggle.isOn = false;
            }

            if (_kinToggle.isOn)
            {
                _kScore++;
                _kinToggle.isOn = false;
            }
        
        }

        private void UpdateSessionScores()
        {
            MasterManager.Instance.visualScore = _vScore;
            MasterManager.Instance.auralScore = _aScore;
            MasterManager.Instance.rwScore = _rScore;
            MasterManager.Instance.kinesScore = _kScore;
        }
    
    }
}
