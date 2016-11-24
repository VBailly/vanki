using System;

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
