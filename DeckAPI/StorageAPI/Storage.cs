namespace StorageAPI
{
    public abstract class Storage
    {
        public abstract void StoreString(string value);
        public abstract string LoadString();

        public static Storage Instance { get; set; }

    }
}
