using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account.Models
{
    internal class UserData
    {

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("currentAccessory")]
        public string CurrentAccessory { get; set; }

        [JsonProperty("ownedAccessories")]
        public List<string> OwnedAccessories { get; set; }

    }
}
