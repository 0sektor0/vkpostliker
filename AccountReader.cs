using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace vkpostliker
{
    public class UsersList
    {
        [JsonProperty("users", Required = Required.Always)]
        public List<User> Users { get; set; }

        public static UsersList FromJson(string json) => JsonConvert.DeserializeObject<UsersList>(json, vkpostliker.Converter.Settings);
    }



    public class User
    {
        [JsonProperty("login", Required = Required.Always)]
        public string Login { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }



    public static class Serialize
    {
        public static string ToJson(this UsersList self) => JsonConvert.SerializeObject(self, vkpostliker.Converter.Settings);
    }



    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }


    public class AccountReader
    {
        static public UsersList GetUsers(string filename)
        {
            if(!File.Exists(filename))
                throw new FileNotFoundException($"{filename} not found");

            using(StreamReader sr = new StreamReader(filename))
            return UsersList.FromJson(sr.ReadToEnd());
        }
    }
}
