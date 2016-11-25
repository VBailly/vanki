using System;
using System.Collections.Generic;
using System.Linq;
using DeckAPI;


namespace Vanki
{

    public abstract class State
    {
        protected readonly IDeck deck;
        protected readonly DateTime now;
        protected readonly IVerbalMessages verbalMessages;

        protected State(IDeck deck, DateTime now, IVerbalMessages verbalMessages)
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
        public abstract string ProcessAnswer(string answer);
        public abstract string PrintNextQuestion();
        public abstract string RevertLastWrongAnswer(bool add);
    }
    
}
