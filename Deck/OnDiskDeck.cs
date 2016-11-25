using System;
using System.Collections.Generic;
using DeckAPI;


public class OnDiskDeck : IDisposableDeck
{
    readonly IDeck deck;

    public OnDiskDeck()
    {
        deck = Persistence.Load();
    }

    public void Dispose()
    {
        Persistence.Save(deck);
    }

    public string GetClue()
    {
        return deck.GetClue();
    }

    public DateTime GetNextCardDueTime()
    {
        return deck.GetNextCardDueTime();
    }

    public string GetNextQuestion()
    {
        return deck.GetNextQuestion();
    }

    public DeckState GetState(DateTime now)
    {
        return deck.GetState(now);
    }

    public bool NextCardNeedsAClue()
    {
        return deck.NextCardNeedsAClue();
    }

    public DeckOperationResult ProcessAnswer(string answer, DateTime now)
    {
        return deck.ProcessAnswer(answer, now);
    }

    public DeckOperationResult AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
    {
        return deck.AddNewCard(questions, answers, caseSensitive, now);
    }

    public DeckOperationResult RevertLastWrongAnswer(bool add, DateTime now)
    {
        return deck.RevertLastWrongAnswer(add, now);
    }
}
