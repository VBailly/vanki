using System;
using System.Collections.Generic;

public class Card
{
    public Guid Id { get; set; }
    public IList<string> Questions { get; set; }
    public IList<string> Answers { get; set; } = new List<string>();
    public bool CaseSensitiveAnswers { get; set; }
    public int Clue { get; set; }
    public DateTime LastAnswerTime { get; set; }
    public int CurrentInterval { get; set; }

    public DateTime DueTime
    {
        get { return LastAnswerTime + TimeSpan.FromMinutes(CurrentInterval); }
    }


    public void Promote(DateTime answerTime)
    {
        if (CurrentInterval == 0 || Clue != 0)
            CurrentInterval = 2;
        else
            CurrentInterval = Math.Max(2, (int)(1.6 * (answerTime - LastAnswerTime).TotalMinutes));
        LastAnswerTime = answerTime;
        DecreaseClue();
    }

    public void Reset()
    {
        CurrentInterval = 0;
        IncreaseClue();
    }


    public void IncreaseClue()
    {
        Clue += 1;
    }

    public void DecreaseClue()
    {
        if (Clue > 0)
            Clue -= 1;
    }

    public void ResetLapse()
    {
        CurrentInterval = 0;
    }
}

