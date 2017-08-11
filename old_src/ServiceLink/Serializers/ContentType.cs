namespace ServiceLink.Serializers
{
    public class ContentType
    {
        private readonly string _contentType;

        internal ContentType(string contentType)
        {
            _contentType = contentType;
        }

        public override string ToString()
        {
            return _contentType;
        }
        
        public static ContentType Parse(string contentType)
            => new ContentType(contentType);
    }
}