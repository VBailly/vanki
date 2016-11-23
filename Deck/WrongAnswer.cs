using System;

public class WrongAnswer
{
    public static WrongAnswer NoWrongAnswer = new WrongAnswer();

    public Guid QuestionId { get; set; }
    public string Answer { get; set; }
    public int PreviousLapse { get; set; }
}
