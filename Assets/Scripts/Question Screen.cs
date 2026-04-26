using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MCQ_System;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionScreen : MonoBehaviour
{
    // Shows question
    // Stores answers
    // Saves scores
    public static Action<OptionChoice, string> OnAnswerSent;
    [SerializeField] private UnityEvent onComplete;
    
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
    
    
    
    private string _sessionID;
    [SerializeField] private string examID;
    
    [SerializeField] private Button nextPageButton;
    [SerializeField] private Button prevPageButton;

    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text userAnsText;
    [SerializeField] private TMP_Text optionAText;
    [SerializeField] private TMP_Text optionBText;
    [SerializeField] private TMP_Text optionCText;
    [SerializeField] private TMP_Text optionDText;

    [SerializeField] private GameObject options;
    
    [SerializeField] private GameObject bButton;
    [SerializeField] private GameObject cButton;
    
    [SerializeField] private GameObject trueFalseObj;
    
    [SerializeField] private List<QuestionTemplate> questions;
    
    private int _currentQIndex;
    private int _lastQIndex;
    private int _highestQIndexSeen = -1;

    private OptionChoice[] _userAns;
    private OptionChoice[] _trueAns;
    private int _score;


    private void Start()
    {
        _sessionID = MasterManagerV2.Instance.sessionID;
        
        _lastQIndex =  questions.Count - 1;
        
        _userAns = new OptionChoice[questions.Count];
        _trueAns = new OptionChoice[questions.Count];
        for (int i = 0; i < questions.Count; i++)
        {
            _trueAns[i] = questions[i].Ans;
        }
        
        // Colours
        _prefixA = $"<color=#{_colorA.ToHexString()}>A</color> - ";
        _prefixB = $"<color=#{_colorB.ToHexString()}>B</color> - ";
        _prefixC = $"<color=#{_colorC.ToHexString()}>C</color> - ";
        _prefixD = $"<color=#{_colorD.ToHexString()}>D</color> - ";
        
        ChangePage(0);
    }

    private void OnEnable()
    {
        OnAnswerSent += HandleAnswer;
    }

    private void OnDisable()
    {
        OnAnswerSent -= HandleAnswer;
    }

    private void HandleAnswer(OptionChoice answer, string id)
    {
        if (id != examID) return;
        _userAns[_currentQIndex] = answer;
        NextQuestion();
    }

    public void NextQuestion()
    {
        // Add question to seen list
        if (_currentQIndex > _highestQIndexSeen) _highestQIndexSeen = _currentQIndex;
        
        if (_currentQIndex < _lastQIndex)
        {
            _currentQIndex++;
            ChangePage(_currentQIndex);
        } else {
            CalculateScore();
            SaveUserExam();
            Destroy(gameObject);
        }
    }

    public void PrevQuestion()
    {
        if (_currentQIndex <= 0) return;
        _currentQIndex--;
        ChangePage(_currentQIndex);
    }
    
    private void ChangePage(int questionIndex)
    {
        var inRange = questionIndex >= 0 && questionIndex <= _lastQIndex;
        if (!inRange) return;
        
        var currentQuestion = questions[questionIndex];
        questionText.text = currentQuestion.Question;
        
        // Update option text
        if (currentQuestion.QuestionType == QuestionType.TrueFalse)
        {
            ShowMCQOptions(false);
            trueFalseObj.SetActive(true);
        }
        else
        {
            ChangeMCQOptions(currentQuestion);
            ShowMCQOptions(true);
            trueFalseObj.SetActive(false);
        }
        
        if (_currentQIndex <= _highestQIndexSeen)
        {
            // Show prev ans
            ShowUserAns(_currentQIndex);
            ShowNextPageButton(true);
        }
        else
        {
            // HideUserAnsText
            userAnsText.gameObject.SetActive(false);
            ShowNextPageButton(false);
        }
        
        
    }

    private void ShowMCQOptions(bool ShowMCQ)
    {
        // Hide text
        options.SetActive(ShowMCQ);
        
        // Hide Button
        bButton.SetActive(ShowMCQ);
        cButton.SetActive(ShowMCQ);
    }

    private void ChangeMCQOptions(QuestionTemplate question)
    {
        optionAText.text = _prefixA + question.OptA;
        optionBText.text = _prefixB + question.OptB;
        optionCText.text = _prefixC + question.OptC;
        optionDText.text = _prefixD + question.OptD;
    }

    private void ShowUserAns(int questionIndex)
    {
        var userAns = _userAns[questionIndex];
        
        var currentQuestion = questions[questionIndex];

        string ansText = null;
        if (currentQuestion.QuestionType == QuestionType.TrueFalse)
        {
            ansText = userAns == OptionChoice.A ? "True" : "False";
        }
        else
        {
            ansText = userAns.ToString();
        }

        userAnsText.text = $"Choice: {ansText}";
        userAnsText.gameObject.SetActive(true);
    }

    private void ShowNextPageButton(bool showButton)
    {
        nextPageButton.interactable = showButton;
    }
    
    private void CalculateScore()
    {
        var counter = _trueAns.Where((t, i) => _userAns[i] == t).Count();
        _score = counter;
    }

    private void SaveUserExam()
    {
        var savePath = Path.Combine(Application.persistentDataPath, $"{_sessionID}_{examID}_Save.json");
            
        // Enums represented with an integer in save file
        var saveData = new ExamSaveData
        {
            sessionID = _sessionID,
            examID = examID,
            examScore = _score,
            userAnswers = _userAns
        };
            
        var json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            
        File.WriteAllText(savePath, json);
        Debug.LogWarning($"Exam \"{examID}\" data saved at: {savePath}");
    }
}
