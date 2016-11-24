using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{

    public class MyOnlyState : State
    {
        readonly IDeck deck;
        readonly DateTime now;
        readonly IVerbalMessages verbalMessages;

        public MyOnlyState(IDeck deck, DateTime now, IVerbalMessages verbalMessages)
        {
            this.verbalMessages = verbalMessages;
            this.now = now;
            this.deck = deck;
        }

        public override string AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive) 
        { 
            deck.AddNewCard(questions, answers, caseSensitive, now);
            return string.Empty;
        }
        public override string ProcessAnswer(string answer) 
        { 
            if (deck.GetState(now) != DeckState.PendingCard)
                return verbalMessages.NothingToAnswer;

            if (!deck.IsAnswerCorrect(answer))
                deck.SetAnswerWrong(answer, now);
            else
                deck.TreatCorrectAnswer(now);
            return string.Empty;
        }
        public override string PrintNextQuestion() 
        {
            var state = deck.GetState(now);

            if (state == DeckState.Empty)
                return verbalMessages.TheDeckIsEmpty;

            if (state == DeckState.NoPendingCard)
                return WaitABitPresentation();

            return GetQuestionPresentation();
        }

        public override string RevertLastWrongAnswer(bool add) 
        {
            if (!deck.LastAnswerWasWrong())
                return verbalMessages.NothingToRevert;

            return RevertLastAnswer(add);
        }

        string WaitABitPresentation()
        {
            var nextCardTime = deck.GetNextCardDueTime();
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - now) + ")" + "\n";
        }

        string GetQuestionPresentation()
        {
            var question = deck.GetNextQuestion();

            if (!deck.NextCardNeedsAClue())
                return question;

            return question + "\n" + verbalMessages.Clue + ": " + deck.GetHint();
        }

        string RevertLastAnswer(bool add)
        {
            var ret = verbalMessages.RevertLast;

            if (add)
                ret = AddLastAnwerAsCorrect();

            deck.TreatLastAnswerAsCorrect();

            return ret;
        }

        string AddLastAnwerAsCorrect()
        {
            deck.AddLastAnswerAsCorrect();
            return verbalMessages.RevertAddLast;
        }
    }
    
}
