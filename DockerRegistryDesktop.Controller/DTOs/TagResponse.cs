using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockerRegistryDesktop.Controller.DTOs
{
    public class TagResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tags")]
        public string[] Tags { get; set; }

    }
}
