using System.Text.Json.Serialization;

namespace api.Models
{
    public class RepositoryViewModel
    {
        [JsonPropertyName("full_name")]
        public string? FullName { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("owner")]
        public RepositoryOwnerViewModel? Owner { get; set; }
    }

    public class RepositoryOwnerViewModel
    {
        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }
}

