using DeckAPI;
using StorageAPI;

public static class Persistence {

    public static void Save(IDeck theDeck) {

        var xml = SerializationAPI.Serialization.Instance.Serialize(theDeck);
        Storage.Instance.StoreString(xml);
    }


    public static IDeck Load() {
        var content = Storage.Instance.LoadString();
        return SerializationAPI.Serialization.Instance.Deserialize(content) as IDeck;
 
    }
}
