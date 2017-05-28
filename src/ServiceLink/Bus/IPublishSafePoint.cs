using System;

namespace ServiceLink.Bus
{
    public interface IPublishSafePoint
    {
        object SavePublishing(SerializedMessage serialized);
        TimeSpan LeaseInterval { get;  }
        void ProlongateLease(object tag);
        void Complete(object tag);

        (SerializedMessage, object) GetAwaited();
    }
}