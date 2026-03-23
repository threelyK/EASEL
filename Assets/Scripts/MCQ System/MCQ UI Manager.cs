using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MCQ_System
{
    public class MCQUIManager: MonoBehaviour
    {
        // Should handle loads exam page using scriptable obj onto UI
        // Handle UI updates
        
        [SerializeField] private MCQExamTemplate MCQExam; // Which Exam it should load
        private string _examID;
        private string _sessionID;
        
        // UI elements
        [SerializeField] private Button NextPageButton;
        [SerializeField] private Button PrevPageButton;
        
        [SerializeField] private TMP_Text QuestionNumberText;
        [SerializeField] private TMP_Text QuestionText;
        [SerializeField] private TMP_Text ChosenAnswerText;
        [SerializeField] private TMP_Text OptionAText;
        [SerializeField] private TMP_Text OptionBText;
        [SerializeField] private TMP_Text OptionCText;
        [SerializeField] private TMP_Text OptionDText;
        
        // Options
        private Color _colorA =  Color.lightGreen;
        private Color _colorB =  Color.yellowNice;
        private Color _colorC =  Color.dodgerBlue;
        private Color _colorD =  Color.crimson;

        // Option initial texts
        private string _prefixA;
        private string _prefixB;
        private string _prefixC;
        private string _prefixD;
        
        // String loading
        private string[] _questionStrings;
        private string[] _optionAStrings;
        private string[] _optionBStrings;
        private string[] _optionCStrings;
        private string[] _optionDStrings;
        
        // Question tracking
        private int _currentQIndex;
        private int _lastQIndex;
        private List<int> _QsSeen = new ();
        
        // Answer tracking
        private OptionChoice[] _userAnswers;
        private OptionChoice[] _trueAnswers;
        private int _score;
        
        // Actions
        [SerializeField] private UnityEvent OnMCQComplete;
        // public static Action<string> OnMCQComplete;
        public static Action<OptionChoice> OnAnswerSent;
        
        public void PrevPage()
        {
            if (_currentQIndex <= 0) return;
            
            _currentQIndex--;
            UpdatePage(_currentQIndex);
        }

        public void NextPage()
        {
            // Log current page into QsSeen
            _QsSeen.Add(_currentQIndex);
            
            if (_currentQIndex != _lastQIndex)
            {
                // Changing current index
                _currentQIndex++;
                
                // Loading next page
                UpdatePage(_currentQIndex);
            }

            if (_currentQIndex == _lastQIndex)
            {
                // Close Page
                OnMCQComplete?.Invoke(); // For triggering doors
                CalculateScore();
                SaveUserExam();
            }
        }

        private void UpdatePage(int questionIndex)
        {
            // Check question number is within bounds
            bool isWithinBounds = questionIndex >= 0 && questionIndex <= _lastQIndex;
            if (isWithinBounds)
            {
                // Update page
                QuestionNumberText.text = questionIndex == _lastQIndex ? "Last Question" : $"Question #{questionIndex+1}";
                QuestionText.text = _questionStrings[questionIndex];
                
                OptionAText.text =_prefixA + " - " + _optionAStrings[questionIndex];
                OptionBText.text =_prefixB + " - " + _optionBStrings[questionIndex];
                OptionCText.text =_prefixC + " - " + _optionCStrings[questionIndex];
                OptionDText.text =_prefixD + " - " + _optionDStrings[questionIndex];
                
                // Handling if they've seen the question or not before
                if (!_QsSeen.Contains(questionIndex))
                {
                    ChosenAnswerText.text = "";
                    NextPageButton.interactable = false;
                }
                else
                {
                    ChosenAnswerText.text = $"Choice: {_userAnswers[questionIndex]}";
                    NextPageButton.interactable = true;
                }
                
                return;
            }

            throw new IndexOutOfRangeException("questionNum: " + questionIndex + " is out of bounds!");
        }

        private void HandleAnswer(OptionChoice choice)
        {
            _userAnswers[_currentQIndex] = choice;
            NextPage();
        }

        private void CalculateScore()
        {
            var counter = _trueAnswers.Where((t, i) => _userAnswers[i] == t).Count();
            _score = counter;
        }
        
        ///
        /// <summary>Saves sessionID, examID, examScore, examAns</summary>
        ///
        public void SaveUserExam()
        {
            var savePath = Path.Combine(Application.persistentDataPath, $"{_sessionID}_{_examID}_Save.json");
            
            // Enums represented with an integer
            var saveData = new ExamSaveData
            {
                sessionID = _sessionID,
                examID = _examID,
                examScore = _score,
                userAnswers = _userAnswers
            };
            
            var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            
            File.WriteAllText(savePath, json);
            Debug.LogWarning($"Exam \"{_examID}\" data saved at: {savePath}");
        }

        private void Start()
        {
            var questionCount = MCQExam.questions.Length;
            _examID = MCQExam.ExamID;
            _sessionID = MasterManager.Instance.SessionID;
            
            // Colours
            _prefixA = $"<color=#{_colorA.ToHexString()}>A</color>";
            _prefixB = $"<color=#{_colorB.ToHexString()}>B</color>";
            _prefixC = $"<color=#{_colorC.ToHexString()}>C</color>";
            _prefixD = $"<color=#{_colorD.ToHexString()}>D</color>";

            _lastQIndex = questionCount-1;
            
            _questionStrings =  new string[questionCount];
            _optionAStrings = new string[questionCount];
            _optionBStrings = new string[questionCount];
            _optionCStrings = new string[questionCount];
            _optionDStrings = new string[questionCount];
            
            // Answers
            _userAnswers = new OptionChoice[questionCount];
            _trueAnswers = new OptionChoice[questionCount];

            for (int i = 0; i < questionCount; i++)
            {
                _trueAnswers[i] = MCQExam.questions[i].answer;
                
                _questionStrings[i] = MCQExam.questions[i].question;
                _optionAStrings[i] = MCQExam.questions[i].optionA;
                _optionBStrings[i] = MCQExam.questions[i].optionB;
                _optionCStrings[i] = MCQExam.questions[i].optionC;
                _optionDStrings[i] = MCQExam.questions[i].optionD;
            }

            NextPageButton.interactable = false;
            UpdatePage(0);
        }

        // TODO Will cause issues with multiple UI managers
        private void OnEnable()
        {
            OnAnswerSent += HandleAnswer;
        }
        
        private void OnDisable()
        {
            OnAnswerSent -= HandleAnswer;
        }
    }

    [Serializable]
    public class ExamSaveData
    {
        
        [JsonRequired] public string sessionID;
        [JsonRequired] public string examID;
        [JsonRequired] public int examScore;
        [JsonRequired] public OptionChoice[] userAnswers;
    }
}
