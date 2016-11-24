using System;

namespace Vanki
{
    public class EmptyState : State
    {
        public EmptyState(IDeck deck, DateTime now, IVerbalMessages verbalMessages) 
            : base(deck, now, verbalMessages) { }

        public override string ProcessAnswer(string answer)
        {
            return verbalMessages.NothingToAnswer;
        }

        public override string PrintNextQuestion()
        {
            return verbalMessages.TheDeckIsEmpty;
        }

        public override string RevertLastWrongAnswer(bool add)
        {
            return verbalMessages.NothingToRevert;
        }
    }
}
