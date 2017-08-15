﻿using System.Net.Mime;

namespace ServiceLink.Serialization
{
    public class Serialized<TFormat>
    {
        public Serialized(string typeCode, ContentType contentType, TFormat data)
        {
            TypeCode = typeCode;
            ContentType = contentType;
            Data = data;
        }

        public string TypeCode { get; }
        public ContentType ContentType { get; }
        public TFormat Data { get; }
    }
}