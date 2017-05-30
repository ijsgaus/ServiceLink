namespace ServiceLink.Bus
{
    public interface IPublishSafePoint
    {
        ILease SavePublishing(SerializedMessage serialized);
    }
}