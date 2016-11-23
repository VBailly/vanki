using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    public IList<ICard> Cards { get; set; } = new List<ICard>();
    public LastAnswer LastAnswer { get; internal set; } = LastAnswer.NullAnswer;

    public ICard GetNextCardBefore(DateTime time)
    {
        return Cards.Where(c => ((Card)c).DueTime <= time).OrderBy(c => ((Card)c).DueTime).FirstOrDefault();
    }

    public ICard GetNextCard()
    {
        return Cards.OrderBy(c => ((Card)c).DueTime).FirstOrDefault();
    }

    public void AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
    {
        Cards.Add(new Card(questions, answers, caseSensitive, now));
    }

    public bool IsAnswerExpected(DateTime now)
    {
        return GetNextCardBefore(now) != null;
    }

    public void ResetLastAnswer()
    {
        LastAnswer = LastAnswer.NullAnswer;
    }

    public void SaveLastAnswer(string lastAnswer, DateTime now)
    {
        LastAnswer = new LastAnswer
        {
            Answer = lastAnswer,
            PreviousLapse = ((Card)GetNextCardBefore(now)).CurrentInterval
        };
    }

    public void TreatCorrectAnswer(DateTime now)
    {
        ResetLastAnswer();
        ((Card)GetNextCardBefore(now)).Promote(now);
    }
}
