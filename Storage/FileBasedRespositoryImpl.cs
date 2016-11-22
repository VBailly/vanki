using System.IO;
using System.Text;

namespace Storage
{
    internal class FileBasedRespositoryImpl : RepositoryImpl
    {
        readonly string fileName_ = "db.xml";

        public override void StoreString(string data)
        {
            File.WriteAllText(fileName_, data, Encoding.UTF8);
        }

        public override string LoadString()
        {
            if (!File.Exists(fileName_))
                return string.Empty;

            return File.ReadAllText(fileName_, Encoding.UTF8);
        }
    }
}

