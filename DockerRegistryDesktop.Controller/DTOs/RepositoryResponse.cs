using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockerRegistryDesktop.Controller.DTOs
{ 
    class RepositoryResponse
    {
        [JsonProperty("repositories")]
        public string[] RepositoryNames { get; set; }
    }
}
