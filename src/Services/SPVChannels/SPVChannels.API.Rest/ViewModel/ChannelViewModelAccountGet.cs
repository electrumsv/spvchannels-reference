using System.CommandLine.Parsing;
using SPVChannels.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPVChannels.API.Rest.ViewModel
{
    public class ChannelViewModelAccountGet
    {
        [Required]
        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [Required]
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }

        public ChannelViewModelAccountGet(string accountId, string accountName)
        {
            AccountName = accountName;
            AccountId = accountId;
        }
    }
}
