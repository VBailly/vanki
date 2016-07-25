namespace Storage
{
    public abstract class RepositoryImpl
    {
        public abstract string GetStoredString();
        public abstract void StoreString(string data);
    }
}

