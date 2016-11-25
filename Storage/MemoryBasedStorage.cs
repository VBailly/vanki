using StorageAPI;

public class MemoryBasedStorage : Storage
{
    private string value_;

    public override string LoadString()
    {
        return value_;
    }

    public override void StoreString(string value)
    {
        value_ = value;
    }
}
