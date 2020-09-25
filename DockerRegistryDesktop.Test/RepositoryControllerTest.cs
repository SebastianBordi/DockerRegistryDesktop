using DockerRegistryDesktop.Controller;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DockerRegistryDesktop.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetRepositoriesTest()
        {
            RepositoryController controller = RepositoryController.GetInstance("https://docker.emconsol.com","ususario", "contraseña");
            var repos = await controller.GetRepositoriesAsync();
            Console.Write(repos);
        }
    }
}