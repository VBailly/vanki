using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{
    public class OnDiskDeck : IDeck, IDisposable
    {
        readonly IDeck deck;

        public OnDiskDeck()
        {
            deck = Persistence.Load();
        }

        public void AddLastAnswerAsCorrect()
        {
            deck.AddLastAnswerAsCorrect();
        }

        public void AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now)
        {
            deck.AddNewCard(questions, answers, caseSensitive, now);
        }

        public void Dispose()
        {
            Persistence.Save(deck);
        }

        public string GetHint()
        {
            return deck.GetHint();
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

        public bool IsAnswerCorrect(string answer)
        {
            return deck.IsAnswerCorrect(answer);
        }

        public bool LastAnswerWasWrong()
        {
            return deck.LastAnswerWasWrong();
        }

        public bool NextCardNeedsAClue()
        {
            return deck.NextCardNeedsAClue();
        }

        public void SetAnswerWrong(string answer, DateTime now)
        {
            deck.SetAnswerWrong(answer, now);
        }

        public void TreatCorrectAnswer(DateTime now)
        {
            deck.TreatCorrectAnswer(now);
        }

        public void TreatLastAnswerAsCorrect()
        {
            deck.TreatLastAnswerAsCorrect();
        }
    }
    
}
