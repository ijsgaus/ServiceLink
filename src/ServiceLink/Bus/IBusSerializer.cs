using System.Threading.Tasks;
using ServiceLink.Monads;

namespace ServiceLink.Bus
{
    public interface IBusSerializer
    {
        Result<SerializedMessage> Serialize<T>(T data);
        
    }
}