namespace MCQ_System
{
    public class MCQButtonController : ButtonController
    {
        public OptionChoice optionChoice;

        private protected override void ProcessPress()
        {
            MCQUIManager.OnAnswerSent(optionChoice);
        }
    }
}