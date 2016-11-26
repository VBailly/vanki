namespace SerializationAPI
{
    public abstract class Serialization
    {
        public abstract string Serialize(object instance);
        public abstract object Deserialize(string serial);

        public static Serialization Instance { get; set; }
    }
}
