using MCQ_System;
using UnityEngine;

public class QButtonVertical : ButtonVertical
{
    [SerializeField] private OptionChoice optionChoice;
    [SerializeField] private AudioSource qSource;

    private protected override void ProcessPress()
    {
        QuestionScreen.OnAnswerSent?.Invoke(optionChoice, targetID);
        if (qSource is not null) qSource.Play();
    }
}
