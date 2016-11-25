using System;
using System.Collections.Generic;
using DeckAPI;


namespace Vanki
{

    public class SpokenGame
    {
        protected readonly IDeck deck;
        protected readonly DateTime now;
        protected readonly IVerbalMessages verbalMessages;

        public SpokenGame(IDeck deck, DateTime now, IVerbalMessages verbalMessages)
        {
            this.deck = deck;
            this.now = now;
            this.verbalMessages = verbalMessages;
        }

        public string AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive)      
        {
            deck.AddNewCard(questions, answers, caseSensitive, now);
            return string.Empty;
        }
        public string ProcessAnswer(string answer)
        {
            var result = deck.ProcessAnswer(answer, now);
            if (result == DeckOperationResult.NothingToAnswer)
                return verbalMessages.NothingToAnswer;

            return string.Empty;
        }

        public string PrintNextQuestion()
        {
            var deckState = deck.GetState(now);

            if (deckState == DeckState.Empty)
                return verbalMessages.TheDeckIsEmpty;

            if (deckState == DeckState.PendingCard)
            {
              var question = deck.GetNextQuestion();

                if (!deck.NextCardNeedsAClue())
                    return question;

                return question + "\n" + verbalMessages.Clue + ": " + deck.GetClue();
            }

            var nextCardTime = deck.GetNextCardDueTime();
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - now) + ")" + "\n";
        }

        public string RevertLastWrongAnswer(bool add)
        {
            var result = deck.RevertLastWrongAnswer(add, now);
            if (result == DeckOperationResult.NothingToRevert)
                return verbalMessages.NothingToRevert;
            if (result == DeckOperationResult.RevertLast)
                return verbalMessages.RevertLast;
            if (result == DeckOperationResult.RevertAddLast)
                return verbalMessages.RevertAddLast;
            
            return string.Empty;
        }
    }
    
}
