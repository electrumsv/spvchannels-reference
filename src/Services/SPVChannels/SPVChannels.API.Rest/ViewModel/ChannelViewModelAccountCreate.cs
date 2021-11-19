using SPVChannels.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPVChannels.API.Rest.ViewModel
{
    public class ChannelViewModelAccountCreate
    {
        [Required]
        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [Required]
        [JsonPropertyName("username")]
        public string AccountUsername { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string AccountPassword { get; set; }
    }
}
