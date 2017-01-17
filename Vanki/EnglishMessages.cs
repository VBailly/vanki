namespace Vanki
{
    public class EnglishMessages : IVerbalMessages
    {
        public string ThereIsNoNextQuestion => "There is no next question";
        public string NothinggToAnswer => "You cannot answer because there is no question pending";
        public string TheDeckIsEmpty => "There is no questions, the deck is empty";
        public string NothingToRevert => "No last wrong answer to revert";
        public string RevertLast => "Your last answer has been considered correct for this time";
        public string RevertAddLast => "Your last answer has been approved as being correct";
        public string WrongCmdArgs => "wrong command line arguments";
        public string Clue => "clue";
        public string ComeBackAtThisTime => "Come back at this time";
        public string In => "in";
    }
}
