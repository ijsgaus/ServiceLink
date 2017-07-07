namespace ServiceLink

type Serialized<'fmt> = {
    TypeCode : TypeCode
    ContentType : ContentType
    Data : 'fmt
}

type RawSerialized = Serialized<byte array>
type StringSerialized = Serialized<string>


