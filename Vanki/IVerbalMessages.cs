namespace Vanki
{
    public interface IVerbalMessages
    {
        string ThereIsNoNextQuestion { get; }
        string NothingToAnswer { get; }
        string TheDeckIsEmpty { get; }
        string NothingToRevert { get; }
        string RevertLast { get; }
        string RevertAddLast { get; }
        string WrongCmdArgs { get; }
        string Clue { get; }
        string ComeBackAtThisTime { get; }
        string In { get; }
    }
}
