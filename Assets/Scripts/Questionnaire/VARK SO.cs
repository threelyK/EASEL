using UnityEngine;

namespace Questionnaire
{
    [CreateAssetMenu(fileName = "VARKSO", menuName = "Scriptable Objects/UI/VARK")]
    public class VARKSO : ScriptableObject
    {
        public VARKQSO[] VARK;
    }
}
