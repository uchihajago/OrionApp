using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HorizonCore
{
    public interface ISerializerJSON
    {
        string Serialize<T>(T value);
        T Deserialize<T>(HttpContent content);
        T Deserialize<T>(string json);
    }
}
