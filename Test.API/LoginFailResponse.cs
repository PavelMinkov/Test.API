using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
