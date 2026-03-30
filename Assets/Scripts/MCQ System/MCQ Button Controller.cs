using UnityEngine;

namespace MCQ_System
{
    public class MCQButtonController : ButtonController
    {
        public OptionChoice optionChoice;
        [SerializeField] private AudioSource mcqSource;

        private protected override void ProcessPress()
        {
            MCQUIManager.OnAnswerSent(optionChoice, targetID);
            if (mcqSource) mcqSource.Play();
        }
    }
}