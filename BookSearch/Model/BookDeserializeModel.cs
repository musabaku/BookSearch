//using System.Text.Json.Serialization;

//namespace BookSearch.Model
//{
//    public class BookResponse
//    {
//        [JsonPropertyName("items")]
//        public List<Book> Items { get; set; }

//    }
//    public class Book
//    {
//        public String Id { get; set; }
//        public VolumeInfo VolumeInfo { get; set; }

//    }

//    public class VolumeInfo
//    {
//        public String Title { get; set; }
//        public string FirstAuthor => Authors?.FirstOrDefault() ?? "Unknown Author";
//        public List<String> Authors { get; set; }

//        public String PublishedDate { get; set; }
//        public ImageLinks ImageLinks { get; set; }
//    }
//    public class ImageLinks
//    {

//        public string Medium { get; set; }

//    }

//}

using System.Text.Json.Serialization;

namespace BookSearch.Model
{
    public class BookResponse
    {
        [JsonPropertyName("items")]
        public List<Book>? Items { get; set; }
    }

    public class Book
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }


        [JsonPropertyName("authors")]
        public List<string>? Authors { get; set; }

        public string FirstAuthor => Authors?.FirstOrDefault() ?? "Unknown Author";

        [JsonPropertyName("publishedDate")]
        public string? PublishedDate { get; set; }

        [JsonPropertyName("imageLinks")]
        public ImageLinks? ImageLinks { get; set; }
    }

    public class ImageLinks
    {
        [JsonPropertyName("smallThumbnail")]
        public string? SmallThumbnail { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }
    }
}


