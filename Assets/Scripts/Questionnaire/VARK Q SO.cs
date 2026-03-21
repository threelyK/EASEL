using UnityEngine;

namespace Questionnaire
{
    [CreateAssetMenu(fileName = "VARKQSO", menuName = "Scriptable Objects/UI/VARK Question")]
    public class VARKQSO : ScriptableObject
    {
        public string question;
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;
    }
}
