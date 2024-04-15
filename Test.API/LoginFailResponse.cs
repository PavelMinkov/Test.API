using Newtonsoft.Json;

namespace Test.API
{
    public class LoginFailResponse
    {
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("total_fail_attempts")]
        public int TotalFailAttempts { get; set; }
    }
}
