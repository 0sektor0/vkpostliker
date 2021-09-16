using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



namespace vkpostliker
{
    public class Settings
    {
        [JsonProperty("users", Required = Required.Always)]
        public List<User> Users { get; set; }

        [JsonProperty("groups", Required = Required.Always)]
        public List<Group> Groups { get; set; }

        public static Settings FromJson(string json) => JsonConvert.DeserializeObject<Settings>(json, vkpostliker.Converter.Settings);
    }



    public class User
    {
        [JsonProperty("login", Required = Required.Always)]
        public string Login { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }



    public class Group
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("offset", Required = Required.Always)]
        public int Offset { get; set; }
    }



    public static class Serialize
    {
        public static string ToJson(this Settings self) => JsonConvert.SerializeObject(self, vkpostliker.Converter.Settings);
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
        static public Settings GetUsers(string filename)
        {
            if(!File.Exists(filename))
                throw new FileNotFoundException($"{filename} not found");

            using(StreamReader sr = new StreamReader(filename))
            return Settings.FromJson(sr.ReadToEnd());
        }
    }
}
