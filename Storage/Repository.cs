namespace Storage
{
    public static class Repository
    {
        static readonly RepositoryImpl DefaultImpl = new FileBasedRespositoryImpl();
        static RepositoryImpl Implementation { get; set; }

        public static void StoreString(string data)
        {
            (Implementation ?? DefaultImpl).StoreString(data);
        }
    }
}

