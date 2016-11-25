using System.IO;
using System.Text;
using StorageAPI;

public class FileBasedStorage : Storage
{
    readonly string fileName_ = "db.xml";

    public override void StoreString(string value)
    {
        File.WriteAllText(fileName_, value, Encoding.UTF8);
    }

    public override string LoadString()
    {
        if (!File.Exists(fileName_))
            return string.Empty;

        return File.ReadAllText(fileName_, Encoding.UTF8);
    }
}

