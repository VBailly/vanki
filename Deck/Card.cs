using System;
using System.Collections.Generic;
using System.Linq;

public interface ICard
{
    IEnumerable<string> Questions { get; }
    IEnumerable<string> Answers { get; }

    DateTime DueTime { get; }

    void Reset();
    void IncreaseClue();
    void DecreaseClue();
    void ResetLapse();
    void PromoteFrom(int previousLapse);
    void AddAnswer(string answer);
    string GetHint();
    bool IsAnswerCorrect(string answer);
    bool NeedsAClue();
}


internal class Card : ICard
{
    internal Card() { }

    public Card(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
    {
        Id = Guid.NewGuid();
        Questions = new List<string>(questions);
        Answers = new List<string>(answers);
        CaseSensitiveAnswers = caseSensitive;
        LastAnswerTime = now;

    }

    internal Guid Id { get; set; }

    public IEnumerable<string> Questions { get; internal set; }
    public IEnumerable<string> Answers { get; internal set; } = new List<string>();

    public bool CaseSensitiveAnswers { get; internal set; }
    public int Clue { get; internal set; }
    public DateTime LastAnswerTime { get; internal set; }
    public int CurrentInterval { get; internal set; }

    public bool IsAnswerCorrect(string answer)
    {
        Func<string, bool> check = GetCheckingFunction(answer);

        return Answers.Any(check);
    }

    public bool NeedsAClue()
    {
        return Clue != 0;
    }

    public Func<string, bool> GetCheckingFunction(string answer)
    {
        if (CaseSensitiveAnswers)
            return a => a == answer;

        return a => a.ToLower() == answer.ToLower();
    }

    public DateTime DueTime
    {
        get { return LastAnswerTime + TimeSpan.FromMinutes(CurrentInterval); }
    }

    public string GetHint()
    {
        var answers = GetFirstAnswer().Split(',').Select(s => s.Trim());
        return string.Join(", ", answers.Select(w => string.Join(" ", w.Split(' ').Select(s => new string(s.Take(Clue).ToArray())))));
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

    public void PromoteFrom(int previousLapse)
    {
        CurrentInterval = previousLapse;
        DecreaseClue();
    }

    public string GetFirstAnswer()
    {
        return ((IList<string>)Answers)[0];
    }

    public void AddAnswer(string answer)
    {
        ((IList<string>)Answers).Add(answer);
    }
}

