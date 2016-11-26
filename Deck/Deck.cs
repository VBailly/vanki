using System;
using System.Collections.Generic;
using System.Linq;
using DeckAPI;


public class Deck : IDeck
{
    public DeckOperationResult AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
    {
        Cards.Add(new Card(questions, answers, caseSensitive, now));
        return DeckOperationResult.CardCreated;
    }

    public DeckState GetState(DateTime now)
    {
        if (IsEmpty())
            return DeckState.Empty;
        if (IsAnswerExpected(now))
            return DeckState.PendingCard;

        return DeckState.NoPendingCard;
    }

    public IList<Card> Cards { get; set; } = new List<Card>();
    public LastAnswer LastAnswer { get; set; } = LastAnswer.NullAnswer;

    private Card GetNextCardBefore(DateTime time)
    {
        return Cards.Where(c => c.DueTime <= time).OrderBy(c => c.DueTime).FirstOrDefault();
    }

    private Card GetNextCard()
    {
        return Cards.OrderBy(c => c.DueTime).FirstOrDefault();
    }

    private bool IsEmpty()
    {
        return !Cards.Any();
    }

    private bool IsAnswerExpected(DateTime now)
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

    void TreatCorrectAnswer(DateTime now)
    {
        GetNextCardBefore(now).Promote(now);
    }

    public DateTime GetNextCardDueTime()
    {
        return GetNextCard().DueTime;
    }

    public string GetNextQuestion()
    {
        return RandomAPI.Random.Instance.PickRandomly(GetNextCard().Questions);

    }

    void SetAnswerWrong(string answer, DateTime now)
    {
        SaveLastAnswer(answer, now);
        GetNextCard().Reset();
    }

    public void AddLastAnswerAsCorrect()
    {
        GetNextCard().AddAnswer(LastAnswer.Answer);
    }

    public string GetClue()
    {
        return GetNextCard().GetHint();
    }

    public void TreatLastAnswerAsCorrect()
    {
        GetNextCard().PromoteFrom(LastAnswer.PreviousLapse);
        LastAnswer = LastAnswer.NullAnswer;
    }

    bool IsAnswerCorrect(string answer)
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

    public DeckOperationResult ProcessAnswer(string answer, DateTime now)
    {
        var state = GetState(now);
        if (state != DeckState.PendingCard)
            return DeckOperationResult.NothingToAnswer;

        if (!IsAnswerCorrect(answer))
            SetAnswerWrong(answer, now);
        else
            TreatCorrectAnswer(now);

        return DeckOperationResult.AnswerProcessed;
    }

    public DeckOperationResult RevertLastWrongAnswer(bool add, DateTime now)
    {
        var state = GetState(now);
        if (state != DeckState.PendingCard)
            return DeckOperationResult.NothingToRevert;

        return RevertLastWrongAnswer(add);
    }

    DeckOperationResult RevertLastWrongAnswer(bool add)
    {
        if (!LastAnswerWasWrong())
            return DeckOperationResult.NothingToRevert;

        return RevertLastAnswer(add);
    }

    DeckOperationResult RevertLastAnswerAndAdd()
    {
        AddLastAnswerAsCorrect();
        TreatLastAnswerAsCorrect();
        return DeckOperationResult.RevertAddLast;
    }

    DeckOperationResult RevertLastAnswer()
    {
        TreatLastAnswerAsCorrect();
        return DeckOperationResult.RevertLast;
    }

    DeckOperationResult RevertLastAnswer(bool add)
    {
        if (add)
            return RevertLastAnswerAndAdd();

        return RevertLastAnswer();
    }

    DeckOperationResult AddLastAnwerAsCorrect()
    {
        AddLastAnswerAsCorrect();
        return DeckOperationResult.RevertAddLast;
    }
}














