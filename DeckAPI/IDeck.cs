using System;
using System.Collections.Generic;

namespace DeckAPI
{
    public interface IDeck
    {
        DateTime GetNextCardDueTime();
        string GetNextQuestion();
        bool NextCardNeedsAClue();
        string GetClue();
        DeckState GetState(DateTime now);

        DeckOperationResult AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive, DateTime now);
        DeckOperationResult ProcessAnswer(string answer, DateTime now);
        DeckOperationResult RevertLastWrongAnswer(bool add, DateTime now);
    
    }

}
