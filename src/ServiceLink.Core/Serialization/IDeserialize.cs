using LanguageExt;

namespace ServiceLink.Serialization
{
    public interface IDeserialize<TFormat>
    {
        Result<T> Deserialize<T>(Serialized<TFormat> data);
    }
}