﻿namespace Storage
{
    public abstract class RepositoryImpl
    {
        public abstract void StoreString(string data);
        public abstract string LoadString();
    }
}

