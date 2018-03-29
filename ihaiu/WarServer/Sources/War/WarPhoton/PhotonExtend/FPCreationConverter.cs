using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using TrueSync;

public class FPCreationConverter : CustomCreationConverter<FP>
{
    public override FP Create(Type objectType)
    {
        return new FP();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is FP)
        {
            writer.WriteValue(((FP)value).AsFloat().ToString());
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {

        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }


        return (FP)Convert.ToSingle(reader.Value);
    }

    public override bool CanConvert(Type objectType)
    {
        if (objectType == typeof(FP))
            return true;

        return false;
    }

    public override bool CanWrite
    {
        get
        {
            return true;
        }
    }

    public override bool CanRead
    {
        get
        {
            return true;
        }
    }


}