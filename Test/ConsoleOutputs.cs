using System;
namespace Test
{
    public static class ConsoleOutputs
    {
        public const string NoNextQuestionMessage = "There is no next question";
        public const string NextQuestionMessage = "The next question is:\n\"{0}\"";
        public const string EmptyDeckMessage = "There is no questions, the deck is empty";
        public const string NothingToRevert = "No last wrong answer to revert";
        public const string RevertLast = "Last wrong answer has been reverted";
        public const string RevertAddLast = "Last wrong answer has been reverted and added as a right answer";
    }
}

