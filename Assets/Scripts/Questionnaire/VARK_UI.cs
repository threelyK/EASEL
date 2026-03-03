using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Questionnaire
{
    
    public class VARK_UI : MonoBehaviour
    {
        private int _visualIndicator;
        private int _auralIndicator;
        private int _readWriteIndicator;
        private int _kinestheticIndicator;
        private int _currentPageNumber;
        private const int MaxPageNumber = 16;

        [SerializeField] private TMP_Text questionText;
        
        [SerializeField] private Toggle optAToggle;
        [SerializeField] private Toggle optBToggle;
        [SerializeField] private Toggle optCToggle;
        [SerializeField] private Toggle optDToggle;
        
        private TMP_Text _optAText;
        private TMP_Text _optBText;
        private TMP_Text _optCText;
        private TMP_Text _optDText;
        
        private Toggle[] _toggles;
        private TMP_Text[] _texts;
        
        #region QuestionnaireStrings
        private string[] _questionTexts =
        {
            "I want to learn about a new project. I would ask for:",
            "I want to find out more about a tour that I am going on. I would:",
            "I want to save more money and to decide between a range of options. I would:",
            "I want to learn how to take better photos. I would:",
            "I have finished a competition or test and I would like some feedback:",
            "When I am learning I:",
            "I want to learn to do something new on a computer. I would:",
            "I am making a history of the area where I live. I would:",
            "I prefer a presenter or a teacher who uses:",
            "A website has a video showing how to make a special graph or chart. There is a person speaking, " +
            "some lists and words describing what to do and some diagrams. I would learn most from:",
            "I am having trouble assembling a piece of furniture that came in parts. I would:",
            "I want to find out about some accommodation. Before visiting, I would want:",
            "I want to be sure I am doing my physiotherapy exercises correctly. I would:",
            "When learning from the Internet I like:",
            "I want to learn how to play a new board game or card game. I would:",
            "When choosing a career or area of study, these are important for me:"
        };
        private readonly string[] _optionAs =
        {
            // Visual
            "Diagrams to show the project stages with charts of benefits and costs.",
            "Use a map and see where the places are.",
            "Use graphs showing different options for different time periods.",
            "Use diagrams showing the camera and what each part does.",
            "Using graphs showing how my performance has improved.",
            "See patterns in things.",
            "Follow the diagrams in a book.",
            "Gather old maps and charts.",
            "Diagrams, charts, maps or graphs.",
            "Seeing the diagrams.",
            "Go through the step-by-step diagrams again to see if I missed something.",
            "A plan showing the rooms and a map of the area.",
            "Study diagrams illustrating the way the exercises should be done.",
            "Interesting design and visual features.",
            "Use the diagrams that explain the various stages, moves and strategies in the game.",
            "Working with designs, maps or charts."
        };
        private readonly string[] _optionBs =
        {
            // Aural
            "An opportunity to discuss the project.",
            "Talk with the person who planned the tour or others who are going on the tour.",
            "Talk with an expert about the options.",
            "Ask questions and talk about the camera and its features.",
            "From somebody who talks it through with me.",
            "Like to talk things through.",
            "Talk with people who know about the program.",
            "Record stories from people talking about old times.",
            "Question and answer, talk, group discussion, or guest speakers.",
            "Listening.",
            "Ask for advice or help from someone else.",
            "A discussion with the owner or manager.",
            "Listen to an explanation about how the exercises should be done.",
            "Podcasts and videos where I can listen to experts.",
            "Listen to somebody explaining it and ask questions.",
            "Communicating with others through discussion."
        };
        private readonly string[] _optionCs =
        {
            // Read/Write
            "A written report describing the main features of the project.",
            "Read about the tour on the itinerary.",
            "Read a print brochure that describes the options in detail.",
            "Use the written instructions about what to do.",
            "Using a written description of my results.",
            "Read books, articles and handouts.",
            "Read the written instructions that came with the program.",
            "Read articles and other information in old newspapers and documents.",
            "Handouts, books, or readings.",
            "Reading the words.",
            "Go through the step-by-step written instructions again to see if I missed something.",
            "A printed description of the rooms and features.",
            "Check a list of important aspects of the exercises to get right.",
            "Detailed articles.",
            "Read the instructions.",
            "Using words well in written communications."
        };
        private readonly string[] _optionDs =
        {
            // Kinesthetic
            "Examples where the project has been used successfully.",
            "Look at details about the highlights and activities on the tour.",
            "Consider examples of each option using my financial information.",
            "Use examples of good and poor photos showing how to improve them.",
            "Using examples from what I have done.",
            "Use examples and applications.",
            "Start using it and learn by trial and error.",
            "Compare historical photos of the area with what is there now.",
            "Demonstrations, models or practical sessions.",
            "Watching the actions.",
            "Try arranging the parts to see how they fit together.",
            "To view a video of the property.",
            "Compare what I am doing with a video demonstration.",
            "Videos showing how to do things.",
            "Watch others play the game before joining in.",
            "Applying my knowledge in real situations.",
        };
        #endregion

        public void NextPage()
        {
            switch (_currentPageNumber)
            {
                case < MaxPageNumber:
                    SubmitIndicators();
                    _currentPageNumber++;
                    LoadPage(_currentPageNumber);
                    break;
                case MaxPageNumber:
                    LoadResultsPage();
                    break;
            }
        }

        private void TogglePageVisible(bool isVisible)
        {
            questionText.alpha = isVisible ? 1 : 0;
            _optAText.alpha = isVisible ? 1 : 0;
            _optBText.alpha = isVisible ? 1 : 0;
            _optCText.alpha = isVisible ? 1 : 0;
            _optDText.alpha = isVisible ? 1 : 0;
        }

        private void LoadPage(int pageNumber)
        {
            questionText.text = _questionTexts[pageNumber];
            _optAText.text = _optionAs[pageNumber];
            _optBText.text = _optionBs[pageNumber];
            _optCText.text = _optionCs[pageNumber];
            _optDText.text = _optionDs[pageNumber];
            
            var positions = new List<int> {12, 0, -12, -24};

            foreach (var toggle in _toggles)
            {
                toggle.isOn = false;
                
                var index = Random.Range(0, positions.Count);
                toggle.transform.localPosition = new Vector3(toggle.transform.localPosition.x, positions[index], toggle.transform.localPosition.z);
                positions.RemoveAt(index);
            }
        }

        private void LoadResultsPage()
        {
            // TODO: Logic for the results display a bar chart and give a short description of each type
        }

        private void SubmitIndicators()
        {
            if (optAToggle.isOn) _visualIndicator++;
            if (optBToggle.isOn) _auralIndicator++;
            if (optCToggle.isOn) _readWriteIndicator++;
            if (optDToggle.isOn) _kinestheticIndicator++;
        }
        
        private void Start()
        {
            _optAText = optAToggle.GetComponentInChildren<TMP_Text>();
            if (_optAText == null) Debug.LogWarning("No option text found for _optAText");
            
            _optBText = optBToggle.GetComponentInChildren<TMP_Text>();
            if (_optBText == null) Debug.LogWarning("No option text found for _optBText");
            
            _optCText = optCToggle.GetComponentInChildren<TMP_Text>();
            if (_optCText == null) Debug.LogWarning("No option text found for _optCText");
            
            _optDText = optDToggle.GetComponentInChildren<TMP_Text>();
            if (_optDText == null) Debug.LogWarning("No option text found for _optDText");
            
            _toggles = new[] {optAToggle, optBToggle, optCToggle, optDToggle};
            _texts = new[] {questionText, _optAText, _optBText, _optCText, _optDText};
        }
    }
}
