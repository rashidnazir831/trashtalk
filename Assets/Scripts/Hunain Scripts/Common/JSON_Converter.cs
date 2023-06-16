namespace TrashTalk
{
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PlayerData
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }

    public partial class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("UserID")]
        public string UserId { get; set; }

        [JsonProperty("FullName")]
        public string FullName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("AuthProvider")]
        public string AuthProvider { get; set; }

        [JsonProperty("Coins")]
        public long Coins { get; set; }

        [JsonProperty("AccessToken")]
        public string AccessToken { get; set; }
    }

    public partial class PlayerData
    {
        public static PlayerData FromJson(string json) => JsonConvert.DeserializeObject<PlayerData>(json, TrashTalk.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PlayerData self) => JsonConvert.SerializeObject(self, TrashTalk.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
