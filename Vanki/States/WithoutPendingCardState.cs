using System;
using DeckAPI;


namespace Vanki
{
    public class WithoutPendingCardState : State
    {

        public WithoutPendingCardState(IDeck deck, DateTime now, IVerbalMessages verbalMessages) 
            : base(deck, now, verbalMessages) { }

        public override string ProcessAnswer(string answer) 
        { 
            return verbalMessages.NothingToAnswer;
        }

        public override string PrintNextQuestion() 
        {
            var nextCardTime = deck.GetNextCardDueTime();
            return verbalMessages.ThereIsNoNextQuestion + "\n" + verbalMessages.ComeBackAtThisTime + ": " + nextCardTime.ToLocalTime() + " (" + verbalMessages.In + " " + (nextCardTime - now) + ")" + "\n";
        }

        public override string RevertLastWrongAnswer(bool add) 
        {
            return verbalMessages.NothingToRevert;
        }
    }
    
}
