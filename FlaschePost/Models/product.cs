namespace FlaschePost.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Product
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("brandName")]
        public string BrandName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("descriptionText")]
        public string DescriptionText { get; set; }

        [JsonProperty("articles")]
        public List<Article> Articles { get; set; }
    }

    public partial class Article
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("pricePerUnitText")]
        public string PricePerUnitText { get; set; }

        [JsonProperty("image")]
        public Uri Image { get; set; }
    }

    public partial class Product
    {
        public static List<Product> FromJson(string json) => JsonConvert.DeserializeObject<List<Product>>(json, FlaschePost.Models.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this List<Product> self) => JsonConvert.SerializeObject(self, FlaschePost.Models.Converter.Settings);
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
