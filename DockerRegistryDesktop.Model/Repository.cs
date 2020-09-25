using System;

namespace DockerRegistryDesktop.Model
{
    public class Repository
    {
        public string Name { get; set; }
        public Tag[] Tags { get; set; }
    }
}
