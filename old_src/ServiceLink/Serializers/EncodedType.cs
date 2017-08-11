namespace ServiceLink.Serializers
{
    public class EncodedType
    {
        private readonly string _encodedType;

        internal EncodedType(string encodedType)
        {
            _encodedType = encodedType;
        }

        public override string ToString()
        {
            return _encodedType;
        }
        
        public static EncodedType Parse(string encodedType)
            => new EncodedType(encodedType);
    }
}