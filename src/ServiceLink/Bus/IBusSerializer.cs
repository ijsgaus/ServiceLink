using System.Threading.Tasks;

namespace ServiceLink.Bus
{
    public interface IBusSerializer
    {
        SerializedMessage Serialize<T>(T data);
        
    }
}