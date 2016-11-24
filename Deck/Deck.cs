using System;
using System.Collections.Generic;
using System.Linq;

public interface IDeck
{
    void AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now);
    bool HasPendingQuestion(DateTime answerTime);
    bool IsAnswerCorrect(string answer);
    void SetAnswerWrong(string answer, DateTime now);
    void TreatCorrectAnswer(DateTime now);
    bool IsEmpty();
    bool IsAnswerExpected(DateTime now);
    DateTime GetNextCardDueTime();
    string GetNextQuestion();
    bool NextCardNeedsAClue();
    string GetHint();
    bool LastAnswerWasWrong();
    void TreatLastAnswerAsCorrect();
    void AddLastAnswerAsCorrect();
}

public class Deck : IDeck
{
    internal IList<Card> Cards { get; set; } = new List<Card>();
    internal LastAnswer LastAnswer { get; set; } = LastAnswer.NullAnswer;

    private Card GetNextCardBefore(DateTime time)
    {
        return Cards.Where(c => c.DueTime <= time).OrderBy(c => c.DueTime).FirstOrDefault();
    }

    private Card GetNextCard()
    {
        return Cards.OrderBy(c => c.DueTime).FirstOrDefault();
    }

    public bool IsEmpty()
    {
        return !Cards.Any();
    }

    public void AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
    {
        Cards.Add(new Card(questions, answers, caseSensitive, now));
    }

    public bool IsAnswerExpected(DateTime now)
    {
        return GetNextCardBefore(now) != null;
    }

    private void SaveLastAnswer(string lastAnswer, DateTime now)
    {
        LastAnswer = new LastAnswer
        {
            Answer = lastAnswer,
            PreviousLapse = GetNextCardBefore(now).CurrentInterval
        };
    }

    public void TreatCorrectAnswer(DateTime now)
    {
        GetNextCardBefore(now).Promote(now);
    }

    public DateTime GetNextCardDueTime()
    {
        return GetNextCard().DueTime;
    }

    public string GetNextQuestion()
    {
        return GetNextCard().Questions.OrderBy(x => Guid.NewGuid()).First();

    }

    public void SetAnswerWrong(string answer, DateTime now)
    {
        SaveLastAnswer(answer, now);
        GetNextCard().Reset();
    }

    public void AddLastAnswerAsCorrect()
    {
        GetNextCard().AddAnswer(LastAnswer.Answer);
    }

    public string GetHint()
    {
        return GetNextCard().GetHint();
    }

    public void TreatLastAnswerAsCorrect()
    {
        GetNextCard().PromoteFrom(LastAnswer.PreviousLapse);
        LastAnswer = LastAnswer.NullAnswer;
    }

    public bool IsAnswerCorrect(string answer)
    {
        return GetNextCard().IsAnswerCorrect(answer);
    }

    public bool NextCardNeedsAClue()
    {
        return GetNextCard().NeedsAClue();
    }

    public bool HasPendingQuestion(DateTime answerTime)
    {
        return GetNextCardBefore(answerTime) != null;
    }
    public bool LastAnswerWasWrong()
    {
        return LastAnswer != LastAnswer.NullAnswer;
    }
}














