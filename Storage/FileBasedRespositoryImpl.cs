using System.IO;

namespace Storage
{
    public class FileBasedRespositoryImpl : RepositoryImpl
    {
        readonly string fileName_ = "db.xml";

        public override void StoreString(string data)
        {
            File.WriteAllText(fileName_, data);
        }
    }
}

