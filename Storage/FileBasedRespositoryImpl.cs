using System.IO;

namespace Storage
{
    public class FileBasedRespositoryImpl : RepositoryImpl
    {
        readonly string fileName_ = "db.xml";

        public override string GetStoredString()
        {
            if (!File.Exists(fileName_))
                return string.Empty;

            return File.ReadAllText(fileName_);
        }

        public override void StoreString(string data)
        {
            File.WriteAllText(fileName_, data);
        }
    }
}

