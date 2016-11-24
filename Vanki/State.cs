using System;
using System.Collections.Generic;
using System.Linq;

namespace Vanki
{

    public abstract class State
    {
        public abstract string AddNewCard(IEnumerable<string> questions, IEnumerable<string> answers, bool caseSensitive);
        public abstract string ProcessAnswer(string answer);
        public abstract string PrintNextQuestion();
        public abstract string RevertLastWrongAnswer(bool add);
    }
    
}
