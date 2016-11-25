namespace UserInterfaceAPI
{
    public abstract class UserInterface
    {
        public abstract void Launch(string[] args);

        public static UserInterface Instance { get; set; }
    }
}
