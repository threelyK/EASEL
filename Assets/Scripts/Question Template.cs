using MCQ_System;
using UnityEngine;

[CreateAssetMenu(fileName = "MCQExamTemplate", menuName = "Scriptable Objects/UI/Question Template")]
public class QuestionTemplate: ScriptableObject
{
    public QuestionType QuestionType;
    public string Question;
    public string OptA; // A = True for TF
    public string OptB; 
    public string OptC;
    public string OptD; // D = False for TF
    public OptionChoice Ans;
}
