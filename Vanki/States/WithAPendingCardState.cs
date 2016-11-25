using System;
using DeckAPI;


namespace Vanki
{
    public class WithAPendingCardState : State
    {

        public WithAPendingCardState(IDeck deck, DateTime now, IVerbalMessages verbalMessages) 
            : base(deck, now, verbalMessages) { }

        public override string ProcessAnswer(string answer)
        {
            deck.ProcessAnswer(answer, now);
            return string.Empty;
        }

        public override string PrintNextQuestion()
        {
            var question = deck.GetNextQuestion();

            if (!deck.NextCardNeedsAClue())
                return question;

            return question + "\n" + verbalMessages.Clue + ": " + deck.GetHint();
        }

        public override string RevertLastWrongAnswer(bool add)
        {
            if (!deck.LastAnswerWasWrong())
                return verbalMessages.NothingToRevert;

            return RevertLastAnswer(add);
        }

        string RevertLastAnswerAndAdd()
        {
            deck.AddLastAnswerAsCorrect();
            deck.TreatLastAnswerAsCorrect();
            return verbalMessages.RevertAddLast;
        }

        string RevertLastAnswer()
        {
            deck.TreatLastAnswerAsCorrect();
            return verbalMessages.RevertLast;
        }

        string RevertLastAnswer(bool add)
        {
            if (add)
                return RevertLastAnswerAndAdd();
            
            return RevertLastAnswer();
        }

        string AddLastAnwerAsCorrect()
        {
            deck.AddLastAnswerAsCorrect();
            return verbalMessages.RevertAddLast;
        }
    }
    
}
